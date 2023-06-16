using Practice.Services.API.Controllers;

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
    }
}
