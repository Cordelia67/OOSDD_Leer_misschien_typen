CREATE DATABASE IF NOT EXISTS typotrainer_db;
USE typotrainer_db;

CREATE TABLE IF NOT EXISTS test_data (
    id INT AUTO_INCREMENT PRIMARY KEY,
    message VARCHAR(255) NOT NULL
);

INSERT INTO test_data (message) VALUES ('In verbinding met de database.');

CREATE USER IF NOT EXISTS 'typotrainer'@'localhost' IDENTIFIED BY 'LeerTypen6';

GRANT CREATE VIEW, DELETE, EXECUTE, INSERT, SELECT, SHOW VIEW, UPDATE
    ON typotrainer_db.* TO 'typotrainer'@'localhost';

FLUSH PRIVILEGES;