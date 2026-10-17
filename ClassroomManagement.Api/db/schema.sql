IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260811204339_Initial Create'
)
BEGIN
    CREATE TABLE [Classrooms] (
        [Id] int NOT NULL IDENTITY,
        [RoomNumber] int NOT NULL,
        CONSTRAINT [PK_Classrooms] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260811204339_Initial Create'
)
BEGIN
    CREATE TABLE [Courses] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NOT NULL,
        [CourseCode] nvarchar(max) NOT NULL,
        [Level] int NOT NULL,
        [Term] int NOT NULL,
        [Credit] real NOT NULL,
        CONSTRAINT [PK_Courses] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260811204339_Initial Create'
)
BEGIN
    CREATE TABLE [Labrooms] (
        [Id] int NOT NULL IDENTITY,
        [RoomNumber] int NOT NULL,
        [Name] nvarchar(max) NULL,
        CONSTRAINT [PK_Labrooms] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260811204339_Initial Create'
)
BEGIN
    CREATE TABLE [LevelTermSections] (
        [Id] int NOT NULL IDENTITY,
        [Level] int NOT NULL,
        [Term] int NOT NULL,
        [Section] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_LevelTermSections] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260811204339_Initial Create'
)
BEGIN
    CREATE TABLE [Sessionals] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NOT NULL,
        [SessionalCode] nvarchar(max) NOT NULL,
        [Level] int NOT NULL,
        [Term] int NOT NULL,
        [Credit] real NOT NULL,
        CONSTRAINT [PK_Sessionals] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260811204339_Initial Create'
)
BEGIN
    CREATE TABLE [Teachers] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NOT NULL,
        [Code] nvarchar(max) NOT NULL,
        [Designation] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Teachers] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260811204339_Initial Create'
)
BEGIN
    CREATE TABLE [ClassroomLevelTermSection] (
        [ClassroomsId] int NOT NULL,
        [LevelTermSectionsId] int NOT NULL,
        CONSTRAINT [PK_ClassroomLevelTermSection] PRIMARY KEY ([ClassroomsId], [LevelTermSectionsId]),
        CONSTRAINT [FK_ClassroomLevelTermSection_Classrooms_ClassroomsId] FOREIGN KEY ([ClassroomsId]) REFERENCES [Classrooms] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_ClassroomLevelTermSection_LevelTermSections_LevelTermSectionsId] FOREIGN KEY ([LevelTermSectionsId]) REFERENCES [LevelTermSections] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260811204339_Initial Create'
)
BEGIN
    CREATE TABLE [ClassSchedules] (
        [Id] int NOT NULL IDENTITY,
        [Day] int NOT NULL,
        [StartTime] time NOT NULL,
        [EndTime] time NOT NULL,
        [Level] int NOT NULL,
        [Term] int NOT NULL,
        [Section] nvarchar(max) NOT NULL,
        [WeekType] nvarchar(max) NULL,
        [ClassroomId] int NULL,
        [LabroomId] int NULL,
        [CourseId] int NULL,
        [SessionalId] int NULL,
        CONSTRAINT [PK_ClassSchedules] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ClassSchedules_Classrooms_ClassroomId] FOREIGN KEY ([ClassroomId]) REFERENCES [Classrooms] ([Id]),
        CONSTRAINT [FK_ClassSchedules_Courses_CourseId] FOREIGN KEY ([CourseId]) REFERENCES [Courses] ([Id]),
        CONSTRAINT [FK_ClassSchedules_Labrooms_LabroomId] FOREIGN KEY ([LabroomId]) REFERENCES [Labrooms] ([Id]),
        CONSTRAINT [FK_ClassSchedules_Sessionals_SessionalId] FOREIGN KEY ([SessionalId]) REFERENCES [Sessionals] ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260811204339_Initial Create'
)
BEGIN
    CREATE TABLE [LabroomSessional] (
        [AllowedSessionalsId] int NOT NULL,
        [LabroomsId] int NOT NULL,
        CONSTRAINT [PK_LabroomSessional] PRIMARY KEY ([AllowedSessionalsId], [LabroomsId]),
        CONSTRAINT [FK_LabroomSessional_Labrooms_LabroomsId] FOREIGN KEY ([LabroomsId]) REFERENCES [Labrooms] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_LabroomSessional_Sessionals_AllowedSessionalsId] FOREIGN KEY ([AllowedSessionalsId]) REFERENCES [Sessionals] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260811204339_Initial Create'
)
BEGIN
    CREATE TABLE [TeacherAssignments] (
        [Id] int NOT NULL IDENTITY,
        [LevelTermSectionId] int NOT NULL,
        [CourseId] int NULL,
        [SessionalId] int NULL,
        CONSTRAINT [PK_TeacherAssignments] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_TeacherAssignments_Courses_CourseId] FOREIGN KEY ([CourseId]) REFERENCES [Courses] ([Id]),
        CONSTRAINT [FK_TeacherAssignments_LevelTermSections_LevelTermSectionId] FOREIGN KEY ([LevelTermSectionId]) REFERENCES [LevelTermSections] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_TeacherAssignments_Sessionals_SessionalId] FOREIGN KEY ([SessionalId]) REFERENCES [Sessionals] ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260811204339_Initial Create'
)
BEGIN
    CREATE TABLE [ClassScheduleTeacher] (
        [ClassesId] int NOT NULL,
        [TeachersId] int NOT NULL,
        CONSTRAINT [PK_ClassScheduleTeacher] PRIMARY KEY ([ClassesId], [TeachersId]),
        CONSTRAINT [FK_ClassScheduleTeacher_ClassSchedules_ClassesId] FOREIGN KEY ([ClassesId]) REFERENCES [ClassSchedules] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_ClassScheduleTeacher_Teachers_TeachersId] FOREIGN KEY ([TeachersId]) REFERENCES [Teachers] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260811204339_Initial Create'
)
BEGIN
    CREATE TABLE [TeacherTeacherAssignment] (
        [AssignedSectionsId] int NOT NULL,
        [TeachersId] int NOT NULL,
        CONSTRAINT [PK_TeacherTeacherAssignment] PRIMARY KEY ([AssignedSectionsId], [TeachersId]),
        CONSTRAINT [FK_TeacherTeacherAssignment_TeacherAssignments_AssignedSectionsId] FOREIGN KEY ([AssignedSectionsId]) REFERENCES [TeacherAssignments] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_TeacherTeacherAssignment_Teachers_TeachersId] FOREIGN KEY ([TeachersId]) REFERENCES [Teachers] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260811204339_Initial Create'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RoomNumber') AND [object_id] = OBJECT_ID(N'[Classrooms]'))
        SET IDENTITY_INSERT [Classrooms] ON;
    EXEC(N'INSERT INTO [Classrooms] ([Id], [RoomNumber])
    VALUES (1, 204),
    (2, 205),
    (3, 304),
    (4, 305),
    (5, 306),
    (6, 308),
    (7, 309),
    (8, 310),
    (9, 311),
    (10, 407),
    (11, 408),
    (12, 402),
    (13, 502),
    (14, 506),
    (15, 507),
    (16, 510),
    (17, 1001)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'RoomNumber') AND [object_id] = OBJECT_ID(N'[Classrooms]'))
        SET IDENTITY_INSERT [Classrooms] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260811204339_Initial Create'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CourseCode', N'Credit', N'Level', N'Name', N'Term') AND [object_id] = OBJECT_ID(N'[Courses]'))
        SET IDENTITY_INSERT [Courses] ON;
    EXEC(N'INSERT INTO [Courses] ([Id], [CourseCode], [Credit], [Level], [Name], [Term])
    VALUES (1, N''EEE 1163'', CAST(3 AS real), 1, N''Introduction to Electrical Engineering'', 1),
    (2, N''CSE 1101'', CAST(3 AS real), 1, N''Structured Programming Language'', 1),
    (3, N''PHY 1131'', CAST(3 AS real), 1, N''Physics'', 1),
    (4, N''ENG 1127'', CAST(3 AS real), 1, N''English'', 1),
    (5, N''MATH 1141'', CAST(3 AS real), 1, N''MATH 1141'', 1),
    (6, N''CSE 1203'', CAST(3 AS real), 1, N''Object Oriented Programming Language I'', 2),
    (7, N''EEE 1269'', CAST(3 AS real), 1, N''Electronic Circuits'', 2),
    (8, N''MATH 1243'', CAST(3 AS real), 1, N''MATH 1243'', 2),
    (9, N''CSE 1201'', CAST(3 AS real), 1, N''Discrete Mathematics'', 2),
    (10, N''HUM 1221'', CAST(2 AS real), 1, N''Bengali Language and Literature'', 2),
    (11, N''CSE 2101'', CAST(3 AS real), 2, N''Digital Logic Design'', 1),
    (12, N''CHEM 2133'', CAST(3 AS real), 2, N''Chemistry'', 1),
    (13, N''MATH 2145'', CAST(3 AS real), 2, N''MATH 2145'', 1),
    (14, N''CSE 2103'', CAST(3 AS real), 2, N''Data Structures and Algorithm I'', 1),
    (15, N''CSE 2105'', CAST(3 AS real), 2, N''Applied Statistics'', 1),
    (16, N''CSE 2201'', CAST(3 AS real), 2, N''Data Structures and Algorithm II'', 2),
    (17, N''MATH 2247'', CAST(3 AS real), 2, N''MATH 2247'', 2),
    (18, N''CSE 2203'', CAST(3 AS real), 2, N''Theory of Computation'', 2),
    (19, N''EEE 2269'', CAST(3 AS real), 2, N''Electrical Drives'', 2),
    (20, N''CSE 2205'', CAST(3 AS real), 2, N''Database Management Systems'', 2),
    (21, N''HUM 2221'', CAST(2 AS real), 2, N''History of Bangladesh'', 2),
    (22, N''CSE 3109'', CAST(3 AS real), 3, N''Compiler'', 1),
    (23, N''CSE 3103'', CAST(3 AS real), 3, N''Microprocessors'', 1),
    (24, N''CSE 3107'', CAST(3 AS real), 3, N''Data Communication'', 1),
    (25, N''CSE 3101'', CAST(3 AS real), 3, N''Software Engineering'', 1),
    (26, N''ME 3181'', CAST(3 AS real), 3, N''Mechanical Engineering'', 1),
    (27, N''CSE 3105'', CAST(3 AS real), 3, N''Computer Architecture'', 1),
    (28, N''CSE 3201'', CAST(3 AS real), 3, N''Artificial Intelligence'', 2),
    (29, N''CSE 3203'', CAST(3 AS real), 3, N''Operating System'', 2),
    (30, N''CSE 3207'', CAST(3 AS real), 3, N''Math Analysis'', 2),
    (31, N''CSE 3205'', CAST(3 AS real), 3, N''Computer Networks'', 2),
    (32, N''CSE 3209'', CAST(3 AS real), 3, N''Information System Design'', 2),
    (33, N''CSE 4139'', CAST(3 AS real), 4, N''Machine Learning'', 1),
    (34, N''CSE 4103'', CAST(3 AS real), 4, N''Computer Graphics'', 1),
    (35, N''HUM 4123'', CAST(3 AS real), 4, N''Engineering Economics'', 1),
    (36, N''CSE 4141'', CAST(3 AS real), 4, N''Object Oriented Software Eng'', 1),
    (37, N''CSE 4101'', CAST(3 AS real), 4, N''Computer Security'', 1),
    (38, N''IPE 4217'', CAST(2 AS real), 4, N''Industrial Management'', 2),
    (39, N''CSE 4251'', CAST(2 AS real), 4, N''Data Warehousing'', 2),
    (40, N''CSE 4245'', CAST(2 AS real), 4, N''Image Processing'', 2),
    (41, N''CSE 4215'', CAST(2 AS real), 4, N''Professional Ethics'', 2),
    (42, N''HUM 4273'', CAST(2 AS real), 4, N''Accounting'', 2);
    INSERT INTO [Courses] ([Id], [CourseCode], [Credit], [Level], [Name], [Term])
    VALUES (43, N''CSE 4249'', CAST(2 AS real), 4, N''VLSI Design'', 2)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CourseCode', N'Credit', N'Level', N'Name', N'Term') AND [object_id] = OBJECT_ID(N'[Courses]'))
        SET IDENTITY_INSERT [Courses] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260811204339_Initial Create'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name', N'RoomNumber') AND [object_id] = OBJECT_ID(N'[Labrooms]'))
        SET IDENTITY_INSERT [Labrooms] ON;
    EXEC(N'INSERT INTO [Labrooms] ([Id], [Name], [RoomNumber])
    VALUES (1, N''Room 014'', 14),
    (2, N''Room 024'', 24),
    (3, N''Room 107'', 107),
    (4, N''Room 108'', 108),
    (5, N''Room 110'', 110),
    (6, N''EEE Lab'', 202),
    (7, N''Room 206'', 206),
    (8, N''CSE Lab 210'', 210),
    (9, N''CSE Lab 302'', 302),
    (10, N''CSE Lab 307'', 307),
    (11, N''CSE Lab 311'', 311),
    (12, N''CSE/CAD Lab 402'', 402),
    (13, N''CSE Lab 411'', 411),
    (14, N''AC Circuit Lab'', 1001),
    (15, N''DC Circuit Lab'', 1002),
    (16, N''AC Circuit Lab'', 1003),
    (17, N''Electronics Lab'', 1004),
    (18, N''Physics Lab'', 1005),
    (19, N''IPE Computer Lab'', 109)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name', N'RoomNumber') AND [object_id] = OBJECT_ID(N'[Labrooms]'))
        SET IDENTITY_INSERT [Labrooms] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260811204339_Initial Create'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Level', N'Section', N'Term') AND [object_id] = OBJECT_ID(N'[LevelTermSections]'))
        SET IDENTITY_INSERT [LevelTermSections] ON;
    EXEC(N'INSERT INTO [LevelTermSections] ([Id], [Level], [Section], [Term])
    VALUES (1, 1, N''A'', 1),
    (2, 1, N''B'', 1),
    (3, 1, N''A'', 2),
    (4, 1, N''B'', 2),
    (5, 2, N''A'', 1),
    (6, 2, N''B'', 1),
    (7, 2, N''A'', 2),
    (8, 2, N''B'', 2),
    (10, 3, N''A'', 1),
    (11, 3, N''B'', 1),
    (12, 3, N''A'', 2),
    (13, 3, N''B'', 2),
    (14, 4, N''A'', 1),
    (15, 4, N''B'', 1),
    (16, 4, N''A'', 2),
    (17, 4, N''B'', 2)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Level', N'Section', N'Term') AND [object_id] = OBJECT_ID(N'[LevelTermSections]'))
        SET IDENTITY_INSERT [LevelTermSections] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260811204339_Initial Create'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Credit', N'Level', N'Name', N'SessionalCode', N'Term') AND [object_id] = OBJECT_ID(N'[Sessionals]'))
        SET IDENTITY_INSERT [Sessionals] ON;
    EXEC(N'INSERT INTO [Sessionals] ([Id], [Credit], [Level], [Name], [SessionalCode], [Term])
    VALUES (1, CAST(1.5 AS real), 1, N''Intro to Computer System Sessional'', N''CSE 1100'', 1),
    (2, CAST(1.5 AS real), 1, N''Structured Programming Language Sessional'', N''CSE 1102'', 1),
    (3, CAST(1.5 AS real), 1, N''Intro to Electrical Engineering Sessional'', N''EEE 1164'', 1),
    (4, CAST(0.75 AS real), 1, N''Physics Sessional'', N''PHY 1132'', 1),
    (5, CAST(1.5 AS real), 1, N''OOP I Sessional'', N''CSE 1204'', 2),
    (6, CAST(1.5 AS real), 1, N''Numerical Methods Sessional'', N''CSE 1208'', 2),
    (7, CAST(0.75 AS real), 1, N''Electronic Circuits Sessional'', N''EEE 1270'', 2),
    (8, CAST(0.75 AS real), 1, N''Engineering Drawing Sessional'', N''CE 1250'', 2),
    (9, CAST(0.75 AS real), 1, N''English Skill Sessional'', N''ENG 1228'', 2),
    (10, CAST(1.5 AS real), 2, N''Digital Logic Design Sessional'', N''CSE 2102'', 1),
    (11, CAST(1.5 AS real), 2, N''Data Structures I Sessional'', N''CSE 2104'', 1),
    (12, CAST(1.5 AS real), 2, N''OOP II Sessional'', N''CSE 2108'', 1),
    (13, CAST(0.75 AS real), 2, N''Software Development Project I'', N''CSE 2100'', 1),
    (14, CAST(1.5 AS real), 2, N''Data Structures II Sessional'', N''CSE 2202'', 2),
    (15, CAST(1.5 AS real), 2, N''Database Sessional'', N''CSE 2206'', 2),
    (16, CAST(0.75 AS real), 2, N''Electrical Drives Sessional'', N''EEE 2270'', 2),
    (17, CAST(0.75 AS real), 3, N''Software Engineering Sessional'', N''CSE 3102'', 1),
    (18, CAST(0.75 AS real), 3, N''Microprocessors Sessional'', N''CSE 3104'', 1),
    (19, CAST(0.75 AS real), 3, N''Compiler Sessional'', N''CSE 3110'', 1),
    (20, CAST(0.75 AS real), 3, N''Software Development Project II'', N''CSE 3100'', 1),
    (21, CAST(0.75 AS real), 3, N''AI Sessional'', N''CSE 3202'', 2),
    (22, CAST(1.5 AS real), 3, N''OS Sessional'', N''CSE 3204'', 2),
    (23, CAST(1.5 AS real), 3, N''Networks Sessional'', N''CSE 3206'', 2),
    (24, CAST(0.75 AS real), 3, N''Info System Design Sessional'', N''CSE 3210'', 2),
    (25, CAST(1.5 AS real), 3, N''Web Engineering Project'', N''CSE 3200'', 2),
    (26, CAST(0.75 AS real), 4, N''Computer Security Sessional'', N''CSE 4102'', 1),
    (27, CAST(0.75 AS real), 4, N''Computer Graphics Sessional'', N''CSE 4104'', 1),
    (28, CAST(0.75 AS real), 4, N''Machine Learning Sessional'', N''CSE 4140'', 1),
    (29, CAST(0.75 AS real), 4, N''OOSE Sessional'', N''CSE 4142'', 1),
    (30, CAST(0.75 AS real), 4, N''Image Processing Sessional'', N''CSE 4246'', 2),
    (31, CAST(0.75 AS real), 4, N''Data Mining Sessional'', N''CSE 4252'', 2)');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Credit', N'Level', N'Name', N'SessionalCode', N'Term') AND [object_id] = OBJECT_ID(N'[Sessionals]'))
        SET IDENTITY_INSERT [Sessionals] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260811204339_Initial Create'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Code', N'Designation', N'Name') AND [object_id] = OBJECT_ID(N'[Teachers]'))
        SET IDENTITY_INSERT [Teachers] ON;
    EXEC(N'INSERT INTO [Teachers] ([Id], [Code], [Designation], [Name])
    VALUES (1, N''TQA'', N''Lecturer'', N''TQA''),
    (2, N''NHC'', N''Lecturer'', N''NHC''),
    (3, N''PHY'', N''Assistant Professor'', N''PHY''),
    (4, N''MAM'', N''Assistant Professor'', N''MAM''),
    (5, N''ENG1'', N''Lecturer'', N''ENG1''),
    (6, N''EEE1'', N''Lecturer'', N''EEE1''),
    (7, N''MA'', N''Lecturer'', N''MA''),
    (8, N''AH'', N''Assistant Professor'', N''AH''),
    (9, N''MH'', N''Lecturer'', N''MH''),
    (10, N''EMH'', N''Lecturer'', N''EMH''),
    (11, N''MATH2'', N''Assistant Professor'', N''MATH2''),
    (12, N''SG'', N''Lecturer'', N''SG''),
    (13, N''MSZ'', N''Lecturer'', N''MSZ''),
    (14, N''AHS'', N''Lecturer'', N''AHS''),
    (15, N''ASM'', N''Lecturer'', N''ASM''),
    (16, N''MSA'', N''Assistant Professor'', N''MSA''),
    (17, N''CHEM'', N''Assistant Professor'', N''CHEM''),
    (18, N''MATH3'', N''Professor'', N''MATH3''),
    (19, N''MZH'', N''Assistant Professor'', N''MZH''),
    (20, N''MO'', N''Lecturer'', N''MO''),
    (21, N''RR'', N''Assistant Professor'', N''RR''),
    (22, N''MATH4'', N''Professor'', N''MATH4''),
    (23, N''ST'', N''Assistant Professor'', N''ST''),
    (24, N''AKZ'', N''Lecturer'', N''AKZ''),
    (25, N''GR'', N''Assistant Professor'', N''GR''),
    (26, N''EAS'', N''Lecturer'', N''EAS''),
    (27, N''EEE2'', N''Lecturer'', N''EEE2''),
    (28, N''MI'', N''Assistant Professor'', N''MI''),
    (29, N''TMM'', N''Lecturer'', N''TMM''),
    (30, N''NAO'', N''Lecturer'', N''NAO''),
    (31, N''NR'', N''Assistant Professor'', N''NR''),
    (32, N''AS'', N''Assistant Professor'', N''AS''),
    (33, N''JA'', N''Lecturer'', N''JA''),
    (34, N''AA'', N''Lecturer'', N''AA''),
    (35, N''AZ'', N''Lecturer'', N''AZ''),
    (36, N''BBA'', N''Lecturer'', N''BBA''),
    (37, N''SA'', N''Lecturer'', N''SA''),
    (38, N''NF1'', N''Lecturer'', N''NF1''),
    (39, N''IPE'', N''Assistant Professor'', N''IPE''),
    (40, N''AIS'', N''Lecturer'', N''AIS''),
    (41, N''RA'', N''Lecturer'', N''RA''),
    (42, N''MAS'', N''Lecturer'', N''MAS'');
    INSERT INTO [Teachers] ([Id], [Code], [Designation], [Name])
    VALUES (43, N''PR'', N''Lecturer'', N''PR''),
    (44, N''ENG2'', N''Lecturer'', N''ENG2''),
    (45, N''HUM1'', N''Lecturer'', N''HUM1''),
    (46, N''HUM2'', N''Lecturer'', N''HUM2''),
    (47, N''ME'', N''Assistant Professor'', N''ME''),
    (48, N''NHS'', N''Lecturer'', N''NHS''),
    (49, N''MATH1'', N''Professor'', N''MATH1'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Code', N'Designation', N'Name') AND [object_id] = OBJECT_ID(N'[Teachers]'))
        SET IDENTITY_INSERT [Teachers] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260811204339_Initial Create'
)
BEGIN
    CREATE INDEX [IX_ClassroomLevelTermSection_LevelTermSectionsId] ON [ClassroomLevelTermSection] ([LevelTermSectionsId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260811204339_Initial Create'
)
BEGIN
    CREATE INDEX [IX_ClassSchedules_ClassroomId] ON [ClassSchedules] ([ClassroomId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260811204339_Initial Create'
)
BEGIN
    CREATE INDEX [IX_ClassSchedules_CourseId] ON [ClassSchedules] ([CourseId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260811204339_Initial Create'
)
BEGIN
    CREATE INDEX [IX_ClassSchedules_LabroomId] ON [ClassSchedules] ([LabroomId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260811204339_Initial Create'
)
BEGIN
    CREATE INDEX [IX_ClassSchedules_SessionalId] ON [ClassSchedules] ([SessionalId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260811204339_Initial Create'
)
BEGIN
    CREATE INDEX [IX_ClassScheduleTeacher_TeachersId] ON [ClassScheduleTeacher] ([TeachersId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260811204339_Initial Create'
)
BEGIN
    CREATE INDEX [IX_LabroomSessional_LabroomsId] ON [LabroomSessional] ([LabroomsId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260811204339_Initial Create'
)
BEGIN
    CREATE INDEX [IX_TeacherAssignments_CourseId] ON [TeacherAssignments] ([CourseId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260811204339_Initial Create'
)
BEGIN
    CREATE INDEX [IX_TeacherAssignments_LevelTermSectionId] ON [TeacherAssignments] ([LevelTermSectionId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260811204339_Initial Create'
)
BEGIN
    CREATE INDEX [IX_TeacherAssignments_SessionalId] ON [TeacherAssignments] ([SessionalId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260811204339_Initial Create'
)
BEGIN
    CREATE INDEX [IX_TeacherTeacherAssignment_TeachersId] ON [TeacherTeacherAssignment] ([TeachersId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260811204339_Initial Create'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260811204339_Initial Create', N'9.0.10');
END;

COMMIT;
GO

