ALTER TABLE "Questions" ADD "VotesSum" integer NOT NULL DEFAULT 0;

ALTER TABLE "Comments" ADD "VotesSum" integer NOT NULL DEFAULT 0;

ALTER TABLE "Answers" ADD "VotesSum" integer NOT NULL DEFAULT 0;
