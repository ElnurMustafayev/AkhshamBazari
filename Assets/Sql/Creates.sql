create database AkhshamBazariDb;

use AkhshamBazariDb;

create table Products (
    [Id] int primary key identity,
    [Name] nvarchar(100),
    [Price] money,
)