namespace StoreRopa.Models.Vo
{
    public class CurrentUser
    {
        private User _currentUser;

        public CurrentUser()
        {
            _currentUser = new User();
        }
        public CurrentUser IdB(Int32 id)
        {
            _currentUser.Id = id;
            return this;
        }
        public CurrentUser FullNameB(string fullName)
        {
            _currentUser.FullName = fullName;
            return this;
        }
        public CurrentUser UserNameB(string userName)
        {
            _currentUser.UserName = userName;
            return this;
        }
        public CurrentUser RolNameB(string rolName)
        {
            _currentUser.RolName = rolName;
            return this;
        }
        public CurrentUser RolIdB(Int32 rolId)
        {
            _currentUser.RolId = rolId;
            return this;
        }

        public CurrentUser IdPersonaB(Int32 idPersona)
        {
            _currentUser.IdPersona = idPersona;
            return this;
        }
        public User Builder()
        {
            return _currentUser;
        }
    }
}
