using Moq;
using StoreDatamodel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace StoreApplication.UnitTests
{
    public class LoginControllerTests
    {


        /// <summary>
        /// form inputs do not match model used
        /// </summary>
        [Fact]
        public void FailedModelValidation()
        {
            var mockStoreRepo = new Mock<IStoreRepository>();
            
        }

    }
}
