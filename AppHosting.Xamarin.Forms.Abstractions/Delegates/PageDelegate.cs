using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppHosting.Xamarin.Forms.Abstractions.Delegates
{
    /// <summary>
    /// A function that can process page.
    /// </summary>
    /// <param name="page">The <see cref="Page"/> for the processing.</param>
    /// <returns>A task that represents the completion of page processing.</returns>
    public delegate Task PageDelegate(Page page);
}