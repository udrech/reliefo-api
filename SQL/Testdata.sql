-- PostgreSQL Testdata
-- TRUNCATE TABLE customers;
INSERT INTO customers (lastname, firstname, date_of_birth, address, city, zipcode, mobile, created_at, updated_at)
VALUES ('Dreher', 'Urs', '1975-09-30 00:00:00', 'Warteggstrasse 10', 'Rorschacherberg', '9404', '079 295 83 06', NOW(), NOW());
INSERT INTO customers (lastname, firstname, date_of_birth, address, city, zipcode, mobile, created_at, updated_at)
VALUES ('Oettli', 'Sabina', '1972-09-28 00:00:00', 'Warteggstrasse 10', 'Rorschacherberg', '9404', '079 285 60 11', NOW(), NOW());

-- TRUNCATE TABLE therapies;
INSERT INTO therapies (name, name_on_receipt, description, duration, price, valid_from, created_at, updated_at)
VALUES ('Massage xy', 'Rücken-, Nackenmassage', 'Ganzkörpermassage, Rücken- / Nackenmassage, Fussreflexzonenmassage, etc.', 60, 90, '2026-01-01 00:00:00', NOW(), NOW());

-- TRUNCATE TABLE appointments;
INSERT INTO appointments (customers_id, therapies_id, appointment_timestamp, created_at, updated_at)
VALUES (1, 1, '2026-04-10 10:00:00', NOW(), NOW());
INSERT INTO appointments (customers_id, therapies_id, appointment_timestamp, created_at, updated_at)
VALUES (2, 1, '2026-04-11 14:00:00', NOW(), NOW());

-- TRUNCATE TABLE medical_histories;
INSERT INTO medical_histories (customers_id, history_timestamp, type, note, created_at, updated_at)
VALUES (1, '2026-04-10 10:00:00', 'Medikamente', 'Blutdrucksenkung', NOW(), NOW());
INSERT INTO medical_histories (customers_id, history_timestamp, type, note, created_at, updated_at)
VALUES (2, '2026-04-11 14:00:00', 'Beschwerde', 'Verspannungen im Schulterbereich, vor allem links', NOW(), NOW());

-- TRUNCATE TABLE receipts;
INSERT INTO receipts (customers_id, receipt_timestamp, file, data, created_at, updated_at)
VALUES (1, '2026-04-10 10:00:00', 'quittung.pdf', '{}', NOW(), NOW());
INSERT INTO receipts (customers_id, receipt_timestamp, file, data, created_at, updated_at)
VALUES (2, '2026-04-11 14:00:00', 'quittung.pdf', '{}', NOW(), NOW());
