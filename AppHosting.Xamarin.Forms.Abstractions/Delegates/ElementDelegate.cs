using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppHosting.Xamarin.Forms.Abstractions.Delegates
{
    /// <summary>
    /// A function that can process element.
    /// </summary>
    /// <param name="element">The <see cref="Element"/> for the processing.</param>
    /// <returns>A task that represents the completion of element processing.</returns>
    public delegate Task ElementDelegate(Element element);
}