CREATE TABLE "Tags" (
    "Id" uuid NOT NULL,
    "Name" text NULL,
    CONSTRAINT "PK_Tags" PRIMARY KEY ("Id")
);

CREATE TABLE "Users" (
    "Id" uuid NOT NULL,
    "Username" text NULL,
    "Email" text NULL,
    "WebsiteUrl" text NULL,
    "AboutMe" text NULL,
    "CreatedOn" timestamp without time zone NOT NULL,
    "LastSeen" timestamp without time zone NOT NULL,
    CONSTRAINT "PK_Users" PRIMARY KEY ("Id")
);

CREATE TABLE "Questions" (
    "Id" uuid NOT NULL,
    "UserId" uuid NOT NULL,
    "Title" text NULL,
    "Body" text NULL,
    "HasAcceptedAnswer" boolean NOT NULL,
    "CreatedOn" timestamp without time zone NOT NULL,
    CONSTRAINT "PK_Questions" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Questions_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);

CREATE TABLE "Answers" (
    "Id" uuid NOT NULL,
    "UserId" uuid NOT NULL,
    "Body" text NULL,
    "IsAcceptedAnswer" boolean NOT NULL,
    "AcceptedOn" timestamp without time zone NULL,
    "CreatedOn" timestamp without time zone NOT NULL,
    "QuestionId" uuid NOT NULL,
    CONSTRAINT "PK_Answers" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Answers_Questions_QuestionId" FOREIGN KEY ("QuestionId") REFERENCES "Questions" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_Answers_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);

CREATE TABLE "QuestionTags" (
    "QuestionId" uuid NOT NULL,
    "TagId" uuid NOT NULL,
    CONSTRAINT "PK_QuestionTags" PRIMARY KEY ("QuestionId", "TagId"),
    CONSTRAINT "FK_QuestionTags_Questions_QuestionId" FOREIGN KEY ("QuestionId") REFERENCES "Questions" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_QuestionTags_Tags_TagId" FOREIGN KEY ("TagId") REFERENCES "Tags" ("Id") ON DELETE CASCADE
);

CREATE TABLE "Comments" (
    "Id" uuid NOT NULL,
    "UserId" uuid NOT NULL,
    "Body" text NULL,
    "CreatedOn" timestamp without time zone NOT NULL,
    "ParentQuestionId" uuid NULL,
    "ParentAnswerId" uuid NULL,
    "OrderNumber" integer NOT NULL,
    CONSTRAINT "PK_Comments" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Comments_Answers_ParentAnswerId" FOREIGN KEY ("ParentAnswerId") REFERENCES "Answers" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_Comments_Questions_ParentQuestionId" FOREIGN KEY ("ParentQuestionId") REFERENCES "Questions" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_Comments_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);

CREATE TABLE "Votes" (
    "Id" uuid NOT NULL,
    "UserId" uuid NOT NULL,
    "CreatedOn" timestamp without time zone NOT NULL,
    "QuestionId" uuid NULL,
    "AnswerId" uuid NULL,
    "CommentId" uuid NULL,
    "VoteType" integer NOT NULL,
    CONSTRAINT "PK_Votes" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Votes_Answers_AnswerId" FOREIGN KEY ("AnswerId") REFERENCES "Answers" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_Votes_Comments_CommentId" FOREIGN KEY ("CommentId") REFERENCES "Comments" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_Votes_Questions_QuestionId" FOREIGN KEY ("QuestionId") REFERENCES "Questions" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_Votes_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_Answers_QuestionId" ON "Answers" ("QuestionId");

CREATE INDEX "IX_Answers_UserId" ON "Answers" ("UserId");

CREATE INDEX "IX_Comments_ParentAnswerId" ON "Comments" ("ParentAnswerId");

CREATE INDEX "IX_Comments_ParentQuestionId" ON "Comments" ("ParentQuestionId");

CREATE INDEX "IX_Comments_UserId" ON "Comments" ("UserId");

CREATE INDEX "IX_Questions_UserId" ON "Questions" ("UserId");

CREATE INDEX "IX_QuestionTags_TagId" ON "QuestionTags" ("TagId");

CREATE INDEX "IX_Votes_AnswerId" ON "Votes" ("AnswerId");

CREATE INDEX "IX_Votes_CommentId" ON "Votes" ("CommentId");

CREATE INDEX "IX_Votes_QuestionId" ON "Votes" ("QuestionId");

CREATE INDEX "IX_Votes_UserId" ON "Votes" ("UserId");
