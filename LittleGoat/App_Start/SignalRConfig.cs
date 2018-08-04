using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(LittleGoat.SignalRConfig))]
namespace LittleGoat
{
    public class SignalRConfig
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}