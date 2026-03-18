namespace src.Enums;

public static class Permissions
{
    // 1. Define the Actions (Constants)
    private const string READ = "read";
    private const string CREATE = "create";
    private const string UPDATE = "update";
    private const string DELETE = "delete";

    private const string USER_RESOURCE = "user";
    private const string ROLE_RESOURCE = "role";
    private const string PRODUCT_RESOURCE = "product";
    private const string ORDER_RESOURCE = "order";
    private const string CUSTOMER_RESOURCE = "customer";
    private const string ANALYTIC_RESOURCE = "analytic";
        
    private const string SEP = "::";

    public static class Users
    {
        public const string Read   = $"{USER_RESOURCE}{SEP}{READ}";   // "user::read"
        public const string Create = $"{USER_RESOURCE}{SEP}{CREATE}"; // "user::create"
        public const string Update = $"{USER_RESOURCE}{SEP}{UPDATE}"; // "user::update"
        public const string Delete = $"{USER_RESOURCE}{SEP}{DELETE}"; // "user::delete"
    }

    public static class Roles
    {
        public const string Read   = $"{ROLE_RESOURCE}{SEP}{READ}";   // "role::read"
        public const string Create = $"{ROLE_RESOURCE}{SEP}{CREATE}"; // "role::create"
        public const string Update = $"{ROLE_RESOURCE}{SEP}{UPDATE}"; // "role::update"
        public const string Delete = $"{ROLE_RESOURCE}{SEP}{DELETE}"; // "role::delete"
    }

    public static class Products
    {
        public const string Read   = $"{PRODUCT_RESOURCE}{SEP}{READ}"; //  "product::read"
        public const string Create = $"{PRODUCT_RESOURCE}{SEP}{CREATE}";// "product::create"
        public const string Update = $"{PRODUCT_RESOURCE}{SEP}{UPDATE}";// "product::update"
        public const string Delete = $"{PRODUCT_RESOURCE}{SEP}{DELETE}";// "product::delete"
    }
    
    public static class Orders
    {
        public const string Read   = $"{ORDER_RESOURCE}{SEP}{READ}"; //  "order::read"
        public const string Create = $"{ORDER_RESOURCE}{SEP}{CREATE}";// "order::create"
        public const string Update = $"{ORDER_RESOURCE}{SEP}{UPDATE}";// "order::update"
        public const string Delete = $"{ORDER_RESOURCE}{SEP}{DELETE}";// "order::delete"
    }
    
    public static class Customers
    {
        public const string Read   = $"{CUSTOMER_RESOURCE}{SEP}{READ}"; //  "customer::read"
        public const string Create = $"{CUSTOMER_RESOURCE}{SEP}{CREATE}";// "customer::create"
        public const string Update = $"{CUSTOMER_RESOURCE}{SEP}{UPDATE}";// "customer::update"
        public const string Delete = $"{CUSTOMER_RESOURCE}{SEP}{DELETE}";// "customer::delete"
    }
    
    public static class Analytics
    {
        public const string Read   = $"{ANALYTIC_RESOURCE}{SEP}{READ}"; //  "analytic::read"
        public const string Create = $"{ANALYTIC_RESOURCE}{SEP}{CREATE}";// "analytic::create"
        public const string Update = $"{ANALYTIC_RESOURCE}{SEP}{UPDATE}";// "analytic::update"
        public const string Delete = $"{ANALYTIC_RESOURCE}{SEP}{DELETE}";// "analytic::delete"
    }
}