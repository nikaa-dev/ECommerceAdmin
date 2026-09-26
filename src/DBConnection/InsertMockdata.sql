-- 1. Insert Categories
INSERT INTO "Categories" ("Id", "Name", "Description") VALUES
('11111111-1111-1111-1111-111111111111', 'Electronics', 'Electronic gadgets and devices'),
('22222222-2222-2222-2222-222222222222', 'Apparel', 'Clothing and accessories'),
('33333333-3333-3333-3333-333333333333', 'Home & Living', 'Furniture and home decor');

-- 2. Insert Products
INSERT INTO "Products" ("Id", "Name", "Description", "Price", "ImageUrl", "Status", "Stock", "CategoryId") VALUES
('44444444-4444-4444-4444-444444444441', 'Wireless Mouse', 'Ergonomic wireless mouse', 25.99, '/images/mouse.png', 1, 150, '11111111-1111-1111-1111-111111111111'),
('44444444-4444-4444-4444-444444444442', 'Mechanical Keyboard', 'RGB Mechanical Keyboard', 89.99, '/images/keyboard.png', 1, 80, '11111111-1111-1111-1111-111111111111'),
('44444444-4444-4444-4444-444444444443', 'Cotton T-Shirt', 'Unisex basic cotton t-shirt', 15.00, '/images/tshirt.png', 1, 200, '22222222-2222-2222-2222-222222222222');

-- 3. Insert Customers
-- Note: We use dummy password hashes here. These users cannot log in, but they are needed so we can create Orders!
INSERT INTO "Customers" ("Id", "Name", "Email", "NormalizedEmail", "PasswordHash", "PhoneNumber", "Created", "IsActive") VALUES
('cust-001', 'John Doe', 'john@example.com', 'JOHN@EXAMPLE.COM', 'dummy-hash-not-for-login', '0123456789', NOW(), true),
('cust-002', 'Jane Smith', 'jane@example.com', 'JANE@EXAMPLE.COM', 'dummy-hash-not-for-login', '0987654321', NOW(), true);

-- 4. Insert Addresses for the Customers
INSERT INTO "Addresses" ("Id", "CustomerId", "Street", "City", "Province", "PostalCode", "Country") VALUES
('55555555-5555-5555-5555-555555555551', 'cust-001', '123 Main St', 'Phnom Penh', 'Phnom Penh', '12000', 'Cambodia'),
('55555555-5555-5555-5555-555555555552', 'cust-002', '456 Market Blvd', 'Siem Reap', 'Siem Reap', '17000', 'Cambodia');

-- 5. Insert Order Statuses
INSERT INTO "OrderStatuses" ("Id", "Name", "Description") VALUES
('66666666-6666-6666-6666-666666666661', 'Pending', 'Order placed, waiting for payment/processing'),
('66666666-6666-6666-6666-666666666662', 'Processing', 'Order is being prepared'),
('66666666-6666-6666-6666-666666666663', 'Shipped', 'Order is out for delivery'),
('66666666-6666-6666-6666-666666666664', 'Delivered', 'Order has been delivered'),
('66666666-6666-6666-6666-666666666665', 'Cancelled', 'Order cancelled');

-- 6. Insert Payment Methods
INSERT INTO "PaymentMethods" ("Id", "Name", "Description") VALUES
('77777777-7777-7777-7777-777777777771', 'Credit Card', 'Visa/Mastercard'),
('77777777-7777-7777-7777-777777777772', 'ABA Pay', 'ABA Bank QR Code'),
('77777777-7777-7777-7777-777777777773', 'Cash on Delivery', 'Pay with cash upon delivery');

-- 7. Insert Payment Statuses
INSERT INTO "PaymentStatuses" ("Id", "Name") VALUES
('88888888-8888-8888-8888-888888888881', 'Pending'),
('88888888-8888-8888-8888-888888888882', 'Completed'),
('88888888-8888-8888-8888-888888888883', 'Failed'),
('88888888-8888-8888-8888-888888888884', 'Refunded');

-- 8. Insert Orders
INSERT INTO "Orders" ("Id", "Code", "OrderStatusId", "ShippingAddressId", "OrderDate", "TotalAmount", "CustomerId") VALUES
('99999999-9999-9999-9999-999999999991', 'ORD-1001', '66666666-6666-6666-6666-666666666662', '55555555-5555-5555-5555-555555555551', NOW(), 115.98, 'cust-001'),
('99999999-9999-9999-9999-999999999992', 'ORD-1002', '66666666-6666-6666-6666-666666666664', '55555555-5555-5555-5555-555555555552', NOW() - INTERVAL '2 days', 15.00, 'cust-002');

-- 9. Insert Order Details (Line Items for the Orders)
INSERT INTO "OrderDetails" ("Id", "OrderId", "ProductId", "Quantity", "Price") VALUES
('aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1', '99999999-9999-9999-9999-999999999991', '44444444-4444-4444-4444-444444444441', 1, 25.99),
('aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2', '99999999-9999-9999-9999-999999999991', '44444444-4444-4444-4444-444444444442', 1, 89.99),
('aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3', '99999999-9999-9999-9999-999999999992', '44444444-4444-4444-4444-444444444443', 1, 15.00);

-- 10. Insert Payments for the Orders
INSERT INTO "Payments" ("Id", "OrderId", "PaymentMethodId", "PaymentStatusId", "PaymentDate", "Amount", "TransactionId") VALUES
('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb1', '99999999-9999-9999-9999-999999999991', '77777777-7777-7777-7777-777777777772', '88888888-8888-8888-8888-888888888882', NOW(), 115.98, 'TXN-ABA-12345'),
('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb2', '99999999-9999-9999-9999-999999999992', '77777777-7777-7777-7777-777777777773', '88888888-8888-8888-8888-888888888882', NOW() - INTERVAL '2 days', 15.00, 'TXN-COD-98765');