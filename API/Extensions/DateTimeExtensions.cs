namespace API.Extensions
{
    public static class DateTimeExtensions
    {
        public static int CalculateAge(this DateTime dob)
        {
            //dob = Date Of Birth
            var today = DateTime.UtcNow;
            var age = today.Year - dob.Year;

            if (dob > today) age= int.Abs(age);
            // no leap years counted yet so its not perfect 
            return age;
        }
    }
}
