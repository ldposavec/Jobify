CREATE TABLE user_type (
    id SERIAL PRIMARY KEY,
    name VARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE "user" (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    surname VARCHAR(100) NOT NULL,
    mail VARCHAR(150) NOT NULL UNIQUE,
    password VARCHAR(255), -- return to not null after registration is complete
    user_type_id INT NOT NULL,
    is_email_verified BOOLEAN DEFAULT FALSE,
    FOREIGN KEY (user_type_id) REFERENCES user_type(id)
);

CREATE TABLE admin (
    id SERIAL PRIMARY KEY,
    user_id INT NOT NULL,
    FOREIGN KEY (user_id) REFERENCES "user"(id) ON DELETE CASCADE
);

CREATE TABLE firm (
    id SERIAL PRIMARY KEY,
    firm_name VARCHAR(200) NOT NULL,
    oib VARCHAR(11) NOT NULL UNIQUE,
    address VARCHAR(255) NOT NULL,
    industry VARCHAR(100)
);

CREATE TABLE employer (
    id SERIAL PRIMARY KEY,
    firm_id INT NOT NULL,
    user_id INT NOT NULL,
    position VARCHAR(100),
    FOREIGN KEY (firm_id) REFERENCES firm(id) ON DELETE CASCADE,
    FOREIGN KEY (user_id) REFERENCES "user"(id) ON DELETE CASCADE
);

CREATE TABLE student (
    id SERIAL PRIMARY KEY,
    user_id INT NOT NULL,
    average_grade DECIMAL(3, 2),
    year_of_study INT,
    jmbag VARCHAR(10) NOT NULL UNIQUE,
    FOREIGN KEY (user_id) REFERENCES "user"(id) ON DELETE CASCADE
);

CREATE TABLE notification (
    id SERIAL PRIMARY KEY,
    user_id INT NOT NULL,
    message TEXT NOT NULL,
    created_at TIMESTAMP DEFAULT NOW(),
    FOREIGN KEY (user_id) REFERENCES "user"(id) ON DELETE CASCADE
);

CREATE TABLE status (
    id SERIAL PRIMARY KEY,
    name VARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE job_ad (
    id SERIAL PRIMARY KEY,
    employer_id INT NOT NULL,
    title VARCHAR(255) NOT NULL,
    description TEXT NOT NULL,
    salary DECIMAL(12, 2) NOT NULL,
    created_at TIMESTAMP DEFAULT NOW(),
    status_id INT NOT NULL,
    FOREIGN KEY (employer_id) REFERENCES employer(id) ON DELETE CASCADE,
    FOREIGN KEY (status_id) REFERENCES status(id)
);

CREATE TABLE job_app (
    id SERIAL PRIMARY KEY,
    job_ad_id INT NOT NULL,
    student_id INT NOT NULL,
    created_at TIMESTAMP DEFAULT NOW(),
    cv_filepath VARCHAR(255) NOT NULL,
    status_id INT NOT NULL,
    FOREIGN KEY (job_ad_id) REFERENCES job_ad(id) ON DELETE CASCADE,
    FOREIGN KEY (student_id) REFERENCES student(id) ON DELETE CASCADE,
    FOREIGN KEY (status_id) REFERENCES status(id)
);

CREATE TABLE job_offer (
    id SERIAL PRIMARY KEY,
    job_app_id INT NOT NULL,
    date TIMESTAMP DEFAULT NOW(),
    status_id INT NOT NULL,
    FOREIGN KEY (job_app_id) REFERENCES job_app(id) ON DELETE CASCADE,
    FOREIGN KEY (status_id) REFERENCES status(id)
);

INSERT INTO user_type (name) VALUES ('Admin'), ('Employer'), ('Student');
INSERT INTO status (name) VALUES ('Approved'), ('Denied'), ('Pending');

INSERT INTO "user" (name, surname, mail, user_type_id, is_email_verified) VALUES
('Alice', 'Smith', 'alice.smith@example.com', 1, FALSE),
('Bob', 'Johnson', 'bob.johnson@example.com', 2, FALSE),
('Charlie', 'Williams', 'charlie.williams@example.com', 2, FALSE),
('David', 'Brown', 'david.brown@example.com', 2, FALSE),
('Eve', 'Jones', 'eve.jones@example.com', 3, FALSE),
('Frank', 'Taylor', 'frank.taylor@example.com', 3, FALSE),
('Grace', 'Anderson', 'grace.anderson@example.com', 3, FALSE),
('Hannah', 'Thomas', 'hannah.thomas@example.com', 3, FALSE),
('Ian', 'Moore', 'ian.moore@example.com', 3, FALSE),
('Jane', 'Doe', 'student.test@gmail.com', 3, FALSE),
('Milan', 'Dukić', 'employeer.test@gmail.com', 2, FALSE);

INSERT INTO admin (user_id) VALUES (1);

INSERT INTO firm (firm_name, oib, address, industry) VALUES
('TechCorp', '12345678901', '123 Tech Street', 'Technology'),
('BuildCo', '23456789012', '456 Construction Lane', 'Construction'),
('Foodies', '34567890123', '789 Culinary Blvd', 'Food'),
('EduLearn', '45678901234', '321 Learning Rd', 'Education'),
('HealthPlus', '56789012345', '654 Wellness Ave', 'Healthcare'),
('EcoWorld', '67890123456', '987 Green Path', 'Environment'),
('FinSecure', '78901234567', '159 Finance Dr', 'Finance'),
('SportsMax', '89012345678', '753 Sports Ct', 'Sports'),
('CreativeHub', '90123456789', '951 Art Way', 'Arts'),
('TravelNest', '01234567890', '852 Travel Ln', 'Tourism'),
('Algebra', '24919984448', 'Gradišćanska 24', 'Education');

INSERT INTO employer (firm_id, user_id, position) VALUES
(1, 2, 'HR Manager'),
(2, 3, 'Project Manager'),
(3, 4, 'Operations Lead'),
(4, 3, 'Training Coordinator'),
(5, 4, 'Clinical Manager'),
(6, 2, 'Sustainability Head'),
(7, 2, 'Finance Advisor'),
(8, 3, 'Team Coach'),
(9, 4, 'Creative Director'),
(10, 3, 'Tour Guide Manager'),
(11, 11, 'Administration Office');

INSERT INTO student (user_id, average_grade, year_of_study, jmbag) VALUES
(5, 4.5, 2, '0012345678'),
(6, 4.3, 3, '0012345679'),
(7, 3.8, 1, '0012345680'),
(8, 3.5, 4, '0012345681'),
(9, 4.0, 2, '0012345682'),
(10, 3.9, 3, '0012345683');



