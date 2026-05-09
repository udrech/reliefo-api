-- PostgreSQL Testdata
INSERT INTO customers (lastname, firstname, date_of_birth, address, city, zipcode, mobile, created_at, updated_at)
VALUES ('Meier', 'Hans', '1975-02-15 00:00:00', 'St. Gallerstrasse 20', 'Rorschach', '9400', '079 548 44 26', NOW(), NOW());
INSERT INTO customers (lastname, firstname, date_of_birth, address, city, zipcode, mobile, created_at, updated_at)
VALUES ('Schmid', 'Barbara', '1967-05-17 00:00:00', 'Wilenrain 16', 'Rorschacherberg', '9404', '079 648 55 48', NOW(), NOW());
INSERT INTO customers (lastname, firstname, date_of_birth, address, city, zipcode, mobile, created_at, updated_at)
VALUES ('Fritsche', 'Tanja', '1995-08-05 00:00:00', 'Hauptstrasse 30', 'Staad', '9422', '079 674 82 32', NOW(), NOW());

-- TRUNCATE TABLE therapies;
INSERT INTO therapies (name, name_on_bill, description, duration, price, valid_from, valid_to, created_at, updated_at)
VALUES ('Entspannungs-Massage', 'Rücken-, Nackenmassage', 'Nacken, Schultergürtel, Rücken, Brust', 60, 90, '2026-01-01 00:00:00', NULL, NOW(), NOW());
INSERT INTO therapies (name, name_on_bill, description, duration, price, valid_from, valid_to, created_at, updated_at)
VALUES ('Entspannungs-Massage kurz', 'Rücken-, Nackenmassage', 'Nacken, Schultergürtel', 30, 50, '2026-01-01 00:00:00', NULL, NOW(), NOW());
INSERT INTO therapies (name, name_on_bill, description, duration, price, valid_from, valid_to, created_at, updated_at)
VALUES ('Fussreflexzonenmassage', 'Rücken-, Nackenmassage', 'Füsse, Beine', 30, 50, '2026-01-01 00:00:00', NULL, NOW(), NOW());
INSERT INTO therapies (name, name_on_bill, description, duration, price, valid_from, valid_to, created_at, updated_at)
VALUES ('Tibetische Massage', 'Rücken-, Nackenmassage', 'Ganzkörpermassage Tibetische-Art', 60, 90, '2026-01-01 00:00:00', NULL, NOW(), NOW());

-- TRUNCATE TABLE appointments;
INSERT INTO appointments (customers_id, therapies_id, bill_id, appointment_timestamp, created_at, updated_at)
VALUES (1, 1, NULL, '2026-05-01 10:00:00', NOW(), NOW());
INSERT INTO appointments (customers_id, therapies_id, bill_id, appointment_timestamp, created_at, updated_at)
VALUES (2, 1, NULL, '2026-05-02 14:00:00', NOW(), NOW());
INSERT INTO appointments (customers_id, therapies_id, bill_id, appointment_timestamp, created_at, updated_at)
VALUES (3, 2, NULL, '2026-05-03 14:00:00', NOW(), NOW());
INSERT INTO appointments (customers_id, therapies_id, bill_id, appointment_timestamp, created_at, updated_at)
VALUES (1, 3, NULL, '2026-05-04 10:00:00', NOW(), NOW());
INSERT INTO appointments (customers_id, therapies_id, bill_id, appointment_timestamp, created_at, updated_at)
VALUES (2, 4, NULL, '2026-05-05 14:00:00', NOW(), NOW());
INSERT INTO appointments (customers_id, therapies_id, bill_id, appointment_timestamp, created_at, updated_at)
VALUES (3, 1, NULL, '2026-05-06 14:00:00', NOW(), NOW());
INSERT INTO appointments (customers_id, therapies_id, bill_id, appointment_timestamp, created_at, updated_at)
VALUES (1, 2, NULL, '2026-05-07 10:00:00', NOW(), NOW());
INSERT INTO appointments (customers_id, therapies_id, bill_id, appointment_timestamp, created_at, updated_at)
VALUES (2, 3, NULL, '2026-05-08 14:00:00', NOW(), NOW());
INSERT INTO appointments (customers_id, therapies_id, bill_id, appointment_timestamp, created_at, updated_at)
VALUES (3, 4, NULL, '2026-05-09 14:00:00', NOW(), NOW());
INSERT INTO appointments (customers_id, therapies_id, bill_id, appointment_timestamp, created_at, updated_at)
VALUES (1, 4, NULL, '2026-05-10 10:00:00', NOW(), NOW());
INSERT INTO appointments (customers_id, therapies_id, bill_id, appointment_timestamp, created_at, updated_at)
VALUES (2, 2, NULL, '2026-05-11 14:00:00', NOW(), NOW());
INSERT INTO appointments (customers_id, therapies_id, bill_id, appointment_timestamp, created_at, updated_at)
VALUES (3, 3, NULL, '2026-05-12 14:00:00', NOW(), NOW());
INSERT INTO appointments (customers_id, therapies_id, bill_id, appointment_timestamp, created_at, updated_at)
VALUES (1, 1, NULL, '2026-05-13 10:00:00', NOW(), NOW());
INSERT INTO appointments (customers_id, therapies_id, bill_id, appointment_timestamp, created_at, updated_at)
VALUES (2, 4, NULL, '2026-05-14 14:00:00', NOW(), NOW());
INSERT INTO appointments (customers_id, therapies_id, bill_id, appointment_timestamp, created_at, updated_at)
VALUES (3, 2, NULL, '2026-05-15 14:00:00', NOW(), NOW());
INSERT INTO appointments (customers_id, therapies_id, bill_id, appointment_timestamp, created_at, updated_at)
VALUES (1, 3, NULL, '2026-05-16 10:00:00', NOW(), NOW());
INSERT INTO appointments (customers_id, therapies_id, bill_id, appointment_timestamp, created_at, updated_at)
VALUES (2, 1, NULL, '2026-05-17 14:00:00', NOW(), NOW());
INSERT INTO appointments (customers_id, therapies_id, bill_id, appointment_timestamp, created_at, updated_at)
VALUES (3, 4, NULL, '2026-05-18 14:00:00', NOW(), NOW());
INSERT INTO appointments (customers_id, therapies_id, bill_id, appointment_timestamp, created_at, updated_at)
VALUES (1, 2, NULL, '2026-05-19 10:00:00', NOW(), NOW());
INSERT INTO appointments (customers_id, therapies_id, bill_id, appointment_timestamp, created_at, updated_at)
VALUES (2, 3, NULL, '2026-05-20 14:00:00', NOW(), NOW());

-- TRUNCATE TABLE medical_history_records;
INSERT INTO medical_history_records (customers_id, history_timestamp, type, note, created_at, updated_at)
VALUES (1, '2026-04-10 10:00:00', 'Medikamente', 'Blutdrucksenkung', NOW(), NOW());
INSERT INTO medical_history_records (customers_id, history_timestamp, type, note, created_at, updated_at)
VALUES (2, '2026-04-11 14:00:00', 'Anamnese', 'Verspannungen im Schulterbereich, vor allem links', NOW(), NOW());
INSERT INTO medical_history_records (customers_id, history_timestamp, type, note, created_at, updated_at)
VALUES (3, '2026-04-12 14:00:00', 'Anamnese', 'Knieschmerzen seit 3 Jahren', NOW(), NOW());

-- TRUNCATE TABLE bills;
INSERT INTO bills (customers_id, filename, data, created_at, updated_at, deleted_at)
VALUES (1, 'quittung.pdf', '{}', NOW(), NOW(), NULL);
INSERT INTO bills (customers_id, filename, data, created_at, updated_at, deleted_at)
VALUES (2, 'quittung.pdf', '{}', NOW(), NOW(), NULL);
INSERT INTO bills (customers_id, filename, data, created_at, updated_at, deleted_at)
VALUES (3, 'quittung.pdf', '{}', NOW(), NOW(), NULL);
