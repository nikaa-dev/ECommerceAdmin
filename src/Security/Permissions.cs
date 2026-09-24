namespace src.Security;

public static class Permissions
{
    public static class Customer
    {
        public const string Read = "Customer::Read";
        public const string Update = "Customer::Update";
        public const string Delete = "Customer::Delete";
    }

    public static class User
    {
        public const string Read = "User::Read";
        public const string Create = "User::Create";
        public const string Update = "User::Update";
        public const string Delete = "User::Delete";
        public const string Export = "User::Export";
    }

    public static class Role
    {
        public const string Read = "Role::Read";
        public const string Create = "Role::Create";
        public const string Update = "Role::Update";
        public const string Delete = "Role::Delete";
        public const string Export = "Role::Export";
    }

    public static class Order
    {
        public const string Read = "Order::Read";
        public const string Export = "Order::Export";
        public const string Print = "Order::Print";
        public const string Update = "Order::Update";
        public const string Delete = "Order::Delete";
    }

    public static class DashboardAnalyze
    {
        public const string Read = "DashboardAnalyze::Read";
    }

    public static class Dashboard
    {
        public const string Read = "Dashboard::Read";
    }

    public static class Product
    {
        public const string Read = "Product::Read";
        public const string Create = "Product::Create";
        public const string Update = "Product::Update";
        public const string Delete = "Product::Delete";
        public const string Export = "Product::Export";
    }
}