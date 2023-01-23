using System.Threading;
using System.Threading.Tasks;

namespace FsccProxy
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await Worker.ExecuteAsync(CancellationToken.None);
        }
    }
}