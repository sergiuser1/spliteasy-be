-- Expense types (predefined values)
CREATE TABLE expense_types (
  id UUID PRIMARY KEY DEFAULT gen_random_uuid (),
  name VARCHAR(50) NOT NULL UNIQUE,
  description TEXT
);

INSERT INTO
  expense_types (id, name, description)
VALUES
  (
    gen_random_uuid (),
    'equal',
    'Split equally among all participants'
  ),
  (
    gen_random_uuid (),
    'exact',
    'Specify exact amounts for each participant'
  ),
  (
    gen_random_uuid (),
    'percentage',
    'Split by percentage for each participant'
  ),
  (
    gen_random_uuid (),
    'adjustment',
    'Add +/- for each participant'
  );

-- Expenses
CREATE TABLE expenses (
  id UUID PRIMARY KEY DEFAULT gen_random_uuid (),
  group_id UUID NOT NULL REFERENCES groups (id) ON DELETE CASCADE,
  user_id UUID NOT NULL REFERENCES users (id) ON DELETE CASCADE,
  expense_type_id UUID NOT NULL REFERENCES expense_types (id),
  amount NUMERIC(10, 2) NOT NULL,
  description TEXT,
  date_incurred TIMESTAMP
  WITH
    TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    created_at TIMESTAMP
  WITH
    TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE expense_splits (
  expense_id UUID NOT NULL REFERENCES expenses (id) ON DELETE CASCADE,
  user_id UUID NOT NULL REFERENCES users (id) ON DELETE CASCADE,
  amount NUMERIC(10, 2) NOT NULL,
  amount_extra NUMERIC(10, 2),
  PRIMARY KEY (expense_id, user_id)
);

-- The amount extra is:
-- * equal: NULL
-- * exact: NULL
-- * percentage: the percentage of the total
-- * adjustment: the + or - of the average
-- Indexes for performance
CREATE INDEX idx_expenses_group_id ON expenses (group_id);

CREATE INDEX idx_expenses_user_id ON expenses (user_id);

CREATE INDEX idx_expense_splits_expense_id ON expense_splits (expense_id);

CREATE INDEX idx_expense_splits_user_id ON expense_splits (user_id);


-- Add trigger to check if the split amount is equal to its parts
ALTER TABLE expenses ADD CONSTRAINT expense_splits_total_check 
CHECK (true) NOT VALID;

CREATE OR REPLACE FUNCTION validate_expense_splits_total()
RETURNS TRIGGER AS $$
DECLARE
  total_split_amount NUMERIC(10, 2);
  expense_amount NUMERIC(10, 2);
BEGIN
  -- Get the expense amount
  SELECT amount INTO expense_amount
  FROM expenses
  WHERE id = NEW.id;
  
  -- Calculate the sum of all splits
  SELECT COALESCE(SUM(amount), 0) INTO total_split_amount
  FROM expense_splits
  WHERE expense_id = NEW.id;
  
  -- Check if totals match
  IF total_split_amount != expense_amount THEN
    RAISE EXCEPTION 'Total split amount (%) does not match expense amount (%)', 
      total_split_amount, expense_amount;
  END IF;
  
  RETURN NEW;
END;
$$ LANGUAGE plpgsql;

-- Trigger that runs after transaction commits to validate the totals match
CREATE CONSTRAINT TRIGGER expense_splits_match_expense
AFTER INSERT OR UPDATE ON expenses
DEFERRABLE INITIALLY DEFERRED
FOR EACH ROW
EXECUTE FUNCTION validate_expense_splits_total();
