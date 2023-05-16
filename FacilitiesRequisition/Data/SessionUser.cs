using FacilitiesRequisition.Models;

namespace FacilitiesRequisition.Data; 

public static class SessionUser {
    public const string UserIdKey = "_UserId";
    
    public static void Login(this ISession session, User user) {
        session.SetInt32(UserIdKey, user.Id);
    }

    public static void Logout(this ISession session) {
        session.Remove(UserIdKey);
    }

    public static User? GetLoggedInUser(this ISession session, DatabaseContext databaseContext) {
        var userId = session.GetInt32(UserIdKey);
        return databaseContext.GetUser(userId ?? -1);
    }

    public static bool IsLoggedIn(this ISession session) {
        var username = session.GetInt32(UserIdKey);
        return username != null;
    }
}

