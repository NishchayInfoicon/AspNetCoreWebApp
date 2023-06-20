using Practice.Foundation.Infrastructure.Interfaces;
using Practice.Services.API.Controllers;
using Practice.Services.APIManagers.Records;
using xUnitTest.Tests.ServicesMoq;

namespace xUnitTest.Tests.Controller
{
    public class PostgresCRUDControllerTest
    {
        [Fact]
        public void PostgresCRUDController_Index_AvailableDatabase()
        {
            //AAA 
            //Arrange
            PostgresCRUDController controller = new PostgresCRUDController(new Practice.Services.APIManagers.PostgresManager());
            bool exprectedresult = true;
            //Act
            bool result = controller.IsDBExists("Practice");
            //Assert
            Assert.Equal(exprectedresult, result);
        }

        [Fact]
        public void UpsertRecord()
        {
            // Arrange
            string responseMessage = "Customer Data added successfully";

            //Act
            IPostgresManager postgresManager = new MoqPostgresManager();

            PostgresCRUDController controller = new PostgresCRUDController(postgresManager);

            Customer customer = new Customer
            {
                first_name = "Nishu",
                last_name = "Tanwar",
                email = "nishuchauhan1@gmail.com",
                company = "Infoicon Technologies Pvt. Ltd.",
                street = "Bank wali gali",
                city = "New Delhi",
                state = "Delhi",
                zip = 110036,
                phone = "8861099544",
                birth_date = DateTime.UtcNow,
                gender = 'M',
                date_entered = DateTime.UtcNow,
                id = 1
            };
            var response = controller.UpsertCustomer(customer);

            //Assert
            Assert.NotNull(response);
            Assert.Equal(responseMessage, response.Message);
            Assert.True(response.IsSuccess);
        }

        [Fact]
        public void DeleteData()
        {
            // Arrange
            string responseMessage = "Deleted 1 row(s).";

            //Act
            IPostgresManager postgresManager = new MoqPostgresManager();

            PostgresCRUDController controller = new PostgresCRUDController(postgresManager);
            var response = controller.DeleteCustomerById("customer", 1);

            //Assert
            Assert.NotNull(response);
            Assert.Equal(responseMessage, response.Message);
            Assert.True(response.IsSuccess);
        }

        [Fact]
        public void ReadDataFromTable_ReturnsListOfItems()
        {
            // Arrange
            var tablename = "customer";

            // Create an instance of the class that contains the ReadDataFromTable metho
            IPostgresManager postgresManager = new MoqPostgresManager();
            PostgresCRUDController controller = new PostgresCRUDController(postgresManager);
            // Act
            var result = controller.Read(tablename);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count); // Assert the expected count based on the mock data

            // Assert the properties of the second item in the result list
            var secondItem = result[0];
            Assert.Equal(1, secondItem.id);
            Assert.Equal("Nishu", secondItem.first_name);
            Assert.Equal("Tanwar", secondItem.last_name);
            Assert.Equal("nishuchauhan1@gmail.com", secondItem.email);
            Assert.Equal("Infoicon Technologies Pvt. Ltd.", secondItem.company);
            Assert.Equal("Bank wali gali", secondItem.street);
            Assert.Equal("New Delhi", secondItem.city);
            Assert.Equal("Delhi", secondItem.state);
            Assert.Equal(110036, secondItem.zip);
            Assert.Equal("8861099544", secondItem.phone);
            Assert.Equal(DateTime.UtcNow.Date, secondItem.birth_date.Date);
            Assert.Equal('M', secondItem.gender);
            Assert.Equal(DateTime.UtcNow.Date, secondItem.date_entered.Date);

            // Add more assertions as needed for other properties and items
        }
    }
}
