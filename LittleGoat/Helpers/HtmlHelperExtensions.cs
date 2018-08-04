namespace LittleGoat
{
    using LittleGoat.Models;
    using LittleGoat.ViewModels;
    using System.Drawing;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;
    using System.Web.Mvc;

    public static class HtmlHelperExtensions
    {
        public static string GetPlaceHolderLinkForAvatar(string playerName)
        {
            string backgroundHexColor = GetHexColorFromText(playerName);
            string fontHexColor = GetFontHexColorFromBackgroundHexColor(backgroundHexColor);
            playerName = playerName.Substring(0, 1).ToUpper();

            return $"http://placehold.it/50/{backgroundHexColor}/{fontHexColor}&text={playerName}";
        }

        public static string GetPlaceHolderLinkForAvatar(this HtmlHelper html, string playerName)
        {
            return GetPlaceHolderLinkForAvatar(playerName);
        }

        public static string RenderRazorViewToString(Controller controller, string viewName, object model)
        {
            controller.ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
                var viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(controller.ControllerContext, viewResult.View);

                return sw.GetStringBuilder().ToString();
            }
        }

        public static ChatMessageViewModel MapChatMessageInChatMessageViewModel(this HtmlHelper html, ChatMessage chatMessage, string align)
        {
            return new ChatMessageViewModel()
            {
                Align = align,
                Date = chatMessage.Date.ToString("dd/MM HH:mm"),
                Id = chatMessage.Id,
                Message = chatMessage.Message,
                PictureUrl = GetPlaceHolderLinkForAvatar(chatMessage.PlayerName),
                PlayerName = chatMessage.PlayerName
            };
        }

        private static string GetHexColorFromText(string text)
        {
            var hash = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(text));
            var color = Color.FromArgb(hash[0], hash[1], hash[2]);

            return color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
        }

        private static string GetFontHexColorFromBackgroundHexColor(string hexColor)
        {
            Color color = ColorTranslator.FromHtml("#" + hexColor);
            int d = 0;

            // Counting the perceptive luminance - human eye favors green color... 
            double luminance = (0.299 * color.R + 0.587 * color.G + 0.114 * color.B) / 255;

            if (luminance > 0.5)
                d = 0; // bright colors - black font
            else
                d = 255; // dark colors - white font

            var resultColor = Color.FromArgb(d, d, d);

            return resultColor.R.ToString("X2") + resultColor.G.ToString("X2") + resultColor.B.ToString("X2");
        }
    }
}