using System.Collections.Generic;
using System.Threading.Tasks;

namespace doorApp.Interfaces
{
    public interface ICloseApplication
    {
        void closeApplication();
        Task PushAsync();
    }
}
