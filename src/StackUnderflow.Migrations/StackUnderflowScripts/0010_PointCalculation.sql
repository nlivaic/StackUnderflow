CREATE OR REPLACE PROCEDURE public.calculate_points(
	user_id uuid,
	points_amount integer)
LANGUAGE 'plpgsql'
AS $$
BEGIN
    UPDATE "Users"
    SET "Points" = (SELECT "Points" FROM "Users" WHERE "Id" = user_id) + points_amount
    WHERE "Id" = user_id;

    COMMIT;
END;$$
