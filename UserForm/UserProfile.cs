namespace UserForm;

[Serializable]
public class UserProfile
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    
    public UserProfile() { }
    
    public UserProfile(string firstName, string lastName, string email, string phone)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Phone = phone;
    }
    
    public override string ToString()
    {
        return $"{LastName} {FirstName} | {Email} | {Phone}";
    }
}