USE GCEFTest;
GO

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.ROUTINES
		   WHERE ROUTINE_NAME = 'GCEFTestReset')
	DROP PROCEDURE GCEFTestReset;
GO

CREATE PROCEDURE GCEFTestReset AS
BEGIN
	EXEC dbo.RebuildTables;

	INSERT INTO AspNetUsers (Id, Email, EmailConfirmed, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount, UserName, FirstName, LastName, Address1, Address2, City, State, ZipCode) VALUES ('11111111-1111-1111-1111-11111111', 'test.customer1@gmail.com', 1, 0, 0, 0, 0, 'Test Customer1', 'Test', 'Customer1', '123 Test Ln', '', 'Minneapolis', 'Minnesota', '55318'),
																																																				   ('22222222-2222-2222-2222-22222222', 'test.employee1@gmail.com', 1, 0, 0, 0, 0, 'Test Employee1', 'Test', 'Employee1', '234 Employee St', '', 'Minneapolis', 'Minnesota', '55318');

	INSERT INTO AspNetRoles (Id, [Name]) VALUES ('aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa', 'Customer'), ('bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbb', 'Sales'), ('cccccccc-cccc-cccc-cccc-cccccccc', 'Admin');

	INSERT INTO AspNetUserRoles VALUES ('22222222-2222-2222-2222-22222222', 'bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbb');
	
	INSERT INTO PurchaseTypes VALUES ('Cash'), ('Bank Finance'), ('Dealer Finance');
	
	INSERT INTO Makes VALUES ('Mazda'), ('Honda'), ('Ford');

	INSERT INTO Models VALUES ('CX-3', 1), ('CX-5', 1), ('Accord', 2), ('Civic', 2), ('Fit', 2);

	INSERT INTO TransmissionTypes VALUES ('Automatic'), ('Manual'), ('Automated Manual'), ('Continuous Variable');

	INSERT INTO BodyStyles VALUES ('Sedan'), 
								  ('Coupe'), 
								  ('Sports Car'), 
								  ('Station Wagon'), 
								  ('Hatchback'), 
								  ('Convertible'), 
								  ('SUV'), 
								  ('Subcompact'), 
								  ('Minivan'), 
								  ('Van'), 
								  ('Pickup');

	INSERT INTO Vehicles (ModelId, IsNew, BodyStyleId, [Year], TransmissionTypeId, Color, Interior, Mileage, VIN, MSRP, SalePrice, [Description], OwnerId, HighwayMpg, CityMpg, Engine) 
						 VALUES (1, 1, 8, 2020, 1, 'Blue', 'Gray', 75.1, 'WDCGG5HB4FG358810', 35399.99, 35399.99, 'New Carbon Edition Mazda CX-3 is a powerful four-cylinder, front-wheel drive, six speed automatic daily driver. This vehicle has the aura of luxury without the price tag.', '00000000-0000-0000-0000-00000000', 34, 29, 'I-4 2.0 L/122'),
								(2, 1, 8, 2021, 1, 'Green', 'Gray', 75.1, 'WDCGGHB4FG358811', 45399.99, 45399.99, 'New Carbon Edition Mazda CX-5 is a powerful four-cylinder, front wheel drive, six speed automatic daily driver. This vehicle has the aura of luxury without the price tag.', '11111111-1111-1111-1111-11111111', 30, 24, 'I-4 2.5 L/152'),
								(3, 1, 1, 2019, 1, 'Titanium', 'Gray', 2150.2, 'WDCGGHB4FG358812', 27999, 27999, 'It still works, what more do you want?', '00000000-0000-0000-0000-00000000', 38, 30, 'I-4 1.5L/91'),
								(4, 1, 1, 2021, 1, 'Red', 'White', 1121.7, 'WDCGGHB4FG358814', 29899, 29899, 'It is popular, you should probably won one too', '00000000-0000-0000-0000-00000000', 37, 29, 'I-4 2.0L/122'),
								(5, 0, 1, 2021, 1, 'Orange', 'Black', 55.9, 'WDCGG5HB4FG358815', 18999, 18999, 'Please buy this...', null, 40, 33, 'I-4 1.5L/91');

	INSERT INTO Details VALUES (0, 'Brakelights', 'LED Brakelights', 0),
							   (0, 'Spoiler', 'Lip Spoiler', 0),
							   (0, 'Wipers', 'Rain Detecting Variable Intermittent Wipers', 0),
							   (0, 'Spare Wheel', 'Steel Spare Wheel', 0),
							   (0, 'Tires', '215/60R16 AS', 0),
							   (0, 'Wheels', '16x6 Aluminum Alloy Wheels with Silver Accents', 0),
							   (0, 'Paint', 'Clearcoat Paint', 0),
							   (0, 'Rear Window', 'Fixed Rear Window with Fixed Interval Wiper and Defroster', 0),
							   (1, 'Outlet', '1 12V DC Outlet', 0),
							   (1, 'Seatback Storage', '2 Steaback Storage Pocket', 0),
							   (1, 'Seat Recline', '4-Way Passenger Seat Manual Recline',  0),
							   (1, 'Air', 'Air Filtration', 0),
							   (1, 'Air', 'Automatic Air Conditioning', 0),
							   (1, 'Lights', 'Cargo Space Lights', 0),
							   (1, 'Mirror', 'Day/Night Rearview Mirror', 0),
							   (1, 'HUD', 'Heads Up Display', 1),
							   (1, 'Keyless', 'Remote Keyless Entry', 0),
							   (1, 'Keyless', 'Keyless Ignition', 0),
							   (1, 'Tech', 'SMS Text Message Audio Delivery and Reply', 0),
							   (2, 'Monitors', 'Two LCD Monitors in the Front', 0),
							   (2, 'Radio', 'AM/FM Radio', 0),
							   (2, 'Infotainment', 'Infotainment System Voice Command', 1),
							   (2, 'Radio', 'Pandora Internet Radio', 1),
							   (2, 'Bluetooth', 'Bluetooth 5.1', 1),
							   (3, 'Alternator', '100 Amp Alternator', 0),
							   (3, 'Fuel Tank', '12.7 Gallon Fuel Tank', 0),
							   (3, 'Axle', '4.325 Axle Ratio', 0),
							   (3, 'FWD', 'Front Wheel Drive', 0),
							   (4, 'ABS', 'ABS and Driveline Traction Control', 0),
							   (4, 'Airbag', 'Airbag Occupancy Sensor', 0),
							   (4, 'Backup Camera', 'Back-up Camera', 1),
							   (4, 'BSM', 'Blind Spot Monitoring', 1),
							   (4, 'Child Lock', 'Rear Child Safety Locks', 0),
							   (0, 'Wheels', '17 Silver-Painted Alloy', 0),
							   (0, 'Tires', '225/50R17 AS', 0),
							   (3, 'Fuel Tank', '14.8 Gallon Fuel Tank', 0),
							   (3, 'Axle', '3.24 Axle Ratio', 0),
							   (4, 'Lane Departure', 'Lane Departure Warning', 1),
							   (4, 'Impact Beams', 'Side Impact Beams', 1),
							   (2, 'USB', '2 USB Ports', 0),
							   (0, 'Wheels', '15x5.5 Steel Wheels', 0),
							   (0, 'Tires', '185/60R15 84T Tires', 0),
							   (3, 'Fuel Tank', '10.6 Gallon Fuel Tank', 0)
						       ;

	INSERT INTO VehicleDetails VALUES (1, 1), (2, 1), (3, 1), (4, 1), (5, 1), (6, 1), (7, 1), (8, 1), (9, 1), (10, 1), (11, 1), (12, 1), (13, 1), (14, 1), (15, 1), (16, 1), (17, 1), (18, 1), (19, 1), (20, 1), (21, 1), (22, 1), (23, 1), (24, 1), (25, 1), (26, 1), (27, 1), (28, 1), (29, 1), (30, 1), (31, 1), (32, 1), (33, 1),
									  (1, 2), (2, 2), (3, 2), (4, 2), (5, 2), (6, 2), (7, 2), (8, 2), (9, 2), (10, 2), (11, 2), (12, 2), (13, 2), (14, 2), (15, 2), (16, 2), (17, 2), (18, 2), (19, 2), (20, 2), (21, 2), (22, 2), (23, 2), (24, 2), (25, 2), (26, 2), (27, 2), (28, 2), (29, 2), (30, 2), (31, 2), (32, 2), (33, 2), (40, 2),
									  (1, 3), (3, 3), (4, 3), (34, 3), (7, 3), (8, 3), (9, 3), (11, 3), (12, 3), (13, 3), (14, 3), (15, 3), (17, 3), (18, 3), (20, 3), (21, 3), (28, 3), (36, 3), (37, 3), (29, 3), (30, 3), (31, 3), (32, 3), (33, 3), (38, 3), (39, 3), (40, 3),
									  (1, 4), (3, 4), (4, 4), (34, 4), (7, 4), (8, 4), (9, 4), (11, 4), (12, 4), (13, 4), (14, 4), (15, 4), (17, 4), (18, 4), (20, 4), (21, 4), (28, 4), (36, 4), (37, 4), (29, 4), (30, 4), (31, 4), (32, 4), (33, 4), (38, 4), (39, 4),
									  (41, 5), (42, 5), (1, 5), (3, 5), (4, 5), (7, 5), (8, 5), (10, 5), (11, 5), (12, 5), (13, 5), (15, 5), (21, 5), (24, 5), (28, 5), (29, 5), (33, 5)
									  ;
	INSERT INTO ImagePaths ([Path], VehicleId) VALUES ('Mazda/CX5/cx5-1.jpg', 2),
													('Mazda/CX5/cx5-2.jpg', 2),
													('Honda/Civic/civic-1.jpg', 4),
													('Honda/Civic/civic-2.jpg', 4),
													('Honda/Civic/civic-3.jpg', 4),
													('Honda/Fit/fit-1.jpg', 5);

	INSERT INTO SaleRecords (CustomerId, EmployeeId, VehicleId, PurchasePrice, ExpectedSalePrice, [Date], TradeInId, PurchaseTypeId) VALUES ('00000000-0000-0000-0000-00000000', '12345678-1234-1234-1234-12345678', 1, 35399.99, 35399.99, '2016-12-01', 5, 3),
																																		  ('11111111-1111-1111-1111-11111111', '12345678-1234-1234-1234-12345678', 2, 40000, 45399.99, '2014-10-27', null, 1),
																																		  ('00000000-0000-0000-0000-00000000', '12345678-1234-1234-1234-12345678', 4, 29000, 29000, '2014-9-2', 2, 2)
END
GO