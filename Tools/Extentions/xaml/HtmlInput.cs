using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Data.Html;
using Windows.Data.Xml.Dom;
using Windows.Data.Xml.Xsl;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;
using HtmlAgilityPack;
using JetBrains.Annotations;

namespace Tools.Extentions.xaml
{
    /// <summary>
    /// Usage: 
    /// 1) In a XAML file, declare the above namespace, e.g.:
    ///    xmlns:rtbx="using:SocialMediaDashboard.W8.Common"
    ///     
    /// 2) In RichTextBlock controls, set or databind the Html property, e.g.:
    ///    <RichTextBlock rtbx:Properties.Html="{Binding ...}"/>
    ///    or
    ///    <RichTextBlock>
    ///     <rtbx:Properties.Html>
    ///         <![CDATA[
    ///             <p>This is a list:</p>
    ///             <ul>
    ///                 <li>Item 1</li>
    ///                 <li>Item 2</li>
    ///                 <li>Item 3</li>
    ///             </ul>
    ///         ]]>
    ///     </rtbx:Properties.Html>
    /// </RichTextBlock>
    /// </summary>
    public class HtmlInput : DependencyObject
    {
        public static readonly DependencyProperty HtmlProperty =
            DependencyProperty.RegisterAttached("Html", typeof(string), typeof(HtmlInput), new PropertyMetadata(null, HtmlChanged));

        /// <summary>
        /// sets the HTML property
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        public static void SetHtml([NotNull] DependencyObject obj, [NotNull] string value)
        {
            obj.SetValue(HtmlProperty, value);
        }

        /// <summary>
        /// Gets the HTML property
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetHtml([NotNull] DependencyObject obj)
        {
            return (string)obj.GetValue(HtmlProperty);
        }

        /// <summary>
        /// This is called when the HTML has changed so that we can generate RT content
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void HtmlChanged([NotNull] DependencyObject d, [NotNull] DependencyPropertyChangedEventArgs e)
        {
            RichTextBlock richText = d as RichTextBlock;
            if (richText == null) return;

            //Generate blocks
            string xhtml = e.NewValue as string;

            
            string baselink = "";
            //TODO: Ensure this point because: baselink + img.Attributes["src"].Value;
            /*
            if (richText.DataContext is BlogPostDataItem)
            {
                BlogPostDataItem bp = richText.DataContext as BlogPostDataItem;
                baselink = "http://" + bp.Link.Host;
            }*/
            if(xhtml == null) return;
            List<Block> blocks = GenerateBlocksForHtml(xhtml, baselink);

            //Add the blocks to the RichTextBlock
            richText.Blocks.Clear();
            foreach (Block b in blocks)
            {
                richText.Blocks.Add(b);
            }
        }

        private static List<Block> GenerateBlocksForHtml([NotNull] string xhtml, [NotNull] string baselink)
        {
            List<Block> bc = new List<Block>();

            try
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(xhtml);

                foreach (HtmlNode img in doc.DocumentNode.Descendants("img"))
                {
                    if (!img.Attributes["src"].Value.StartsWith("http"))
                    {
                        img.Attributes["src"].Value = baselink + img.Attributes["src"].Value;
                    }
                }

                Block b = GenerateParagraph(doc.DocumentNode);
                bc.Add(b);
            }
            catch (Exception)
            {
                // ignored
            }

            return bc;
        }

        /// <summary>
        /// Cleans HTML text for display in paragraphs
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static string CleanText([NotNull] string input)
        {
            string clean = HtmlUtilities.ConvertToText(input);
            //clean = System.Net.WebUtility.HtmlEncode(clean);
            if (clean == "\0")
                clean = "\n";
            return clean;
        }

        private static Block GenerateBlockForTopNode([NotNull] HtmlNode node)
        {
            return GenerateParagraph(node);
        }


        private static void AddChildren([NotNull] Paragraph p, [NotNull] HtmlNode node)
        {
            bool added = false;
            foreach (HtmlNode child in node.ChildNodes)
            {
                Inline i = GenerateBlockForNode(child);
                if (i != null)
                {
                    p.Inlines.Add(i);
                    added = true;
                }
            }
            if (!added)
            {
                p.Inlines.Add(new Run() { Text = CleanText(node.InnerText) });
            }
        }

        private static void AddChildren([NotNull] Span s, [NotNull] HtmlNode node)
        {
            bool added = false;

            foreach (HtmlNode child in node.ChildNodes)
            {
                Inline i = GenerateBlockForNode(child);
                if (i != null)
                {
                    s.Inlines.Add(i);
                    added = true;
                }
            }
            if (!added)
            {
                s.Inlines.Add(new Run() { Text = CleanText(node.InnerText) });
            }
        }

        private static Inline GenerateBlockForNode([NotNull] HtmlNode node)
        {
            switch (node.Name)
            {
                case "div":
                    return GenerateSpan(node);
                case "p":
                case "P":
                    return GenerateInnerParagraph(node);
                case "img":
                case "IMG":
                    return GenerateImage(node);
                case "a":
                case "A":
                    if (node.ChildNodes.Count >= 1 && (node.FirstChild.Name == "img" || node.FirstChild.Name == "IMG"))
                        return GenerateImage(node.FirstChild);
                    else
                        return GenerateHyperLink(node);
                case "li":
                case "LI":
                    return GenerateLi(node);
                case "b":
                case "B":
                    return GenerateBold(node);
                case "i":
                case "I":
                    return GenerateItalic(node);
                case "u":
                case "U":
                    return GenerateUnderline(node);
                case "br":
                case "BR":
                    return new LineBreak();
                case "span":
                case "Span":
                    return GenerateSpan(node);
                case "iframe":
                case "Iframe":
                    return GenerateIFrame(node);
                case "#text":
                    if (!string.IsNullOrWhiteSpace(node.InnerText))
                        return new Run() { Text = CleanText(node.InnerText) };
                    break;
                case "h1":
                case "H1":
                    return GenerateH1(node);
                case "h2":
                case "H2":
                    return GenerateH2(node);
                case "h3":
                case "H3":
                    return GenerateH3(node);
                case "ul":
                case "UL":
                    return GenerateUl(node);
                default:
                    return GenerateSpanWNewLine(node);
                    //if (!string.IsNullOrWhiteSpace(node.InnerText))
                    //    return new Run() { Text = CleanText(node.InnerText) };
                    //break;
            }
            return null;
        }

        private static Inline GenerateLi([NotNull] HtmlNode node)
        {
            Span s = new Span();
            InlineUIContainer iui = new InlineUIContainer();
            Ellipse ellipse = new Ellipse();
            ellipse.Fill = new SolidColorBrush(Colors.Black);
            ellipse.Width = 6;
            ellipse.Height = 6;
            ellipse.Margin = new Thickness(-30, 0, 0, 1);
            iui.Child = ellipse;
            s.Inlines.Add(iui);
            AddChildren(s, node);
            s.Inlines.Add(new LineBreak());
            return s;
        }

        private static Inline GenerateImage([NotNull] HtmlNode node)
        {
            Span s = new Span();
            try
            {
                InlineUIContainer iui = new InlineUIContainer();
                var sourceUri = WebUtility.HtmlDecode(node.Attributes["src"].Value);
                Image img = new Image() { Source = new BitmapImage(new Uri(sourceUri, UriKind.Absolute)) };
                img.Stretch = Stretch.Uniform;
                img.VerticalAlignment = VerticalAlignment.Top;
                img.HorizontalAlignment = HorizontalAlignment.Left;
                img.ImageOpened += img_ImageOpened;
                img.ImageFailed += img_ImageFailed;
                //img.Tapped += ScrollingBlogPostDetailPage.img_Tapped; //TODO: Ensure this because img_Tapped is important for correct visiblity
                iui.Child = img;
                s.Inlines.Add(iui);
                s.Inlines.Add(new LineBreak());
            }
            catch (Exception)
            {
                // ignored
            }
            return s;
        }

        /*
        public static void img_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            if (ScrollPage != null)
            {
                Image img = sender as Image;
                ScrollPage.imgViewerImg.Source = img.Source;
                ScrollPage.imgViewer.Visibility = Visibility.Visible;
            }
        }*/

        static void img_ImageFailed([NotNull] object sender, [NotNull] ExceptionRoutedEventArgs e)
        {
            var i = 5; //DEBUG Line breaker
        }

        static void img_ImageOpened([NotNull] object sender, [NotNull] RoutedEventArgs e)
        {
            Image img = sender as Image;
            BitmapImage bimg = img?.Source as BitmapImage;
            if (bimg == null) return; 
            if (bimg.PixelWidth > 800 || bimg.PixelHeight > 600)
            {
                img.Width = 800; img.Height = 600;
                if (bimg.PixelWidth > 800)
                {
                    img.Width = 800;
                    img.Height = (800.0 / bimg.PixelWidth) * bimg.PixelHeight;
                }
                if (img.Height > 600)
                {
                    img.Height = 600;
                    img.Width = (600.0 / img.Height) * img.Width;
                }
            }
            else
            {
                img.Height = bimg.PixelHeight;
                img.Width = bimg.PixelWidth;
            }
        }

        private static Inline GenerateHyperLink([NotNull] HtmlNode node)
        {
            Span s = new Span();
            InlineUIContainer iui = new InlineUIContainer();
            HyperlinkButton hb = new HyperlinkButton() { NavigateUri = new Uri(node.Attributes["href"].Value, UriKind.Absolute), Content = CleanText(node.InnerText) };

            if (node.ParentNode != null && (node.ParentNode.Name == "li" || node.ParentNode.Name == "LI"))
                hb.Style = (Style)Application.Current.Resources["RTLinkLI"];
            else if ((node.NextSibling == null || string.IsNullOrWhiteSpace(node.NextSibling.InnerText)) && (node.PreviousSibling == null || string.IsNullOrWhiteSpace(node.PreviousSibling.InnerText)))
                hb.Style = (Style)Application.Current.Resources["RTLinkOnly"];
            else
                hb.Style = (Style)Application.Current.Resources["RTLink"];

            iui.Child = hb;
            s.Inlines.Add(iui);
            return s;
        }

        private static Inline GenerateIFrame([NotNull] HtmlNode node)
        {
            try
            {
                Span s = new Span();
                s.Inlines.Add(new LineBreak());
                InlineUIContainer iui = new InlineUIContainer();
                WebView ww = new WebView() { Source = new Uri(node.Attributes["src"].Value, UriKind.Absolute), Width = Int32.Parse(node.Attributes["width"].Value), Height = Int32.Parse(node.Attributes["height"].Value) };
                iui.Child = ww;
                s.Inlines.Add(iui);
                s.Inlines.Add(new LineBreak());
                return s;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static Block GenerateTopIFrame([NotNull] HtmlNode node)
        {
            try
            {
                Paragraph p = new Paragraph();
                InlineUIContainer iui = new InlineUIContainer();
                WebView ww = new WebView() { Source = new Uri(node.Attributes["src"].Value, UriKind.Absolute), Width = Int32.Parse(node.Attributes["width"].Value), Height = Int32.Parse(node.Attributes["height"].Value) };
                iui.Child = ww;
                p.Inlines.Add(iui);
                return p;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static Inline GenerateBold([NotNull] HtmlNode node)
        {
            Bold b = new Bold();
            AddChildren(b, node);
            return b;
        }

        private static Inline GenerateUnderline([NotNull] HtmlNode node)
        {
            Underline u = new Underline();
            AddChildren(u, node);
            return u;
        }

        private static Inline GenerateItalic([NotNull] HtmlNode node)
        {
            Italic i = new Italic();
            AddChildren(i, node);
            return i;
        }

        private static Block GenerateParagraph([NotNull] HtmlNode node)
        {
            Paragraph p = new Paragraph();
            AddChildren(p, node);
            return p;
        }

        private static Inline GenerateUl([NotNull] HtmlNode node)
        {
            Span s = new Span();
            s.Inlines.Add(new LineBreak());
            AddChildren(s, node);
            return s;
        }

        private static Inline GenerateInnerParagraph([NotNull] HtmlNode node)
        {
            Span s = new Span();
            s.Inlines.Add(new LineBreak());
            AddChildren(s, node);
            s.Inlines.Add(new LineBreak());
            return s;
        }

        private static Inline GenerateSpan([NotNull] HtmlNode node)
        {
            Span s = new Span();
            AddChildren(s, node);
            return s;
        }

        private static Inline GenerateSpanWNewLine([NotNull] HtmlNode node)
        {
            Span s = new Span();
            AddChildren(s, node);
            if (s.Inlines.Count > 0)
                s.Inlines.Add(new LineBreak());
            return s;
        }

        private static Span GenerateH3([NotNull] HtmlNode node)
        {
            Span s = new Span();
            s.Inlines.Add(new LineBreak());
            Bold bold = new Bold();
            Run r = new Run() { Text = CleanText(node.InnerText) };
            bold.Inlines.Add(r);
            s.Inlines.Add(bold);
            s.Inlines.Add(new LineBreak());
            return s;
        }

        private static Inline GenerateH2([NotNull] HtmlNode node)
        {
            Span s = new Span() { FontSize = 24 };
            s.Inlines.Add(new LineBreak());
            Run r = new Run() { Text = CleanText(node.InnerText) };
            s.Inlines.Add(r);
            s.Inlines.Add(new LineBreak());
            return s;
        }

        private static Inline GenerateH1([NotNull] HtmlNode node)
        {
            Span s = new Span() { FontSize = 30 };
            s.Inlines.Add(new LineBreak());
            Run r = new Run() { Text = CleanText(node.InnerText) };
            s.Inlines.Add(r);
            s.Inlines.Add(new LineBreak());
            return s;
        }

        #region old stuff

        private static XsltProcessor _html2XamlProcessor;

        private static async Task<string> ConvertHtmlToXamlRichTextBlock([NotNull] string xhtml)
        {
            // Load XHTML fragment as XML document
            XmlDocument xhtmlDoc = new XmlDocument();
            if (DesignMode.DesignModeEnabled)
            {
                // In design mode we swallow all exceptions to make editing more friendly
                try { xhtmlDoc.LoadXml(xhtml); }
                catch
                {
                    // ignored
                }
                    // For some reason code in catch is not executed when an exception occurs in design mode, so we can't display a friendly error here.
            }
            else
            {
                // When not in design mode, we let the application handle any exceptions
                xhtmlDoc.LoadXml(xhtml);
            }

            if (_html2XamlProcessor == null)
            {
                // Read XSLT. In design mode we cannot access the xslt from the file system (with Build Action = Content), 
                // so we use it as an embedded resource instead:
                Assembly assembly = typeof(HtmlInput).GetTypeInfo().Assembly;
                using (Stream stream = assembly.GetManifestResourceStream("SocialMediaDashboard.W8.Common.RichTextBlockHtml2Xaml.xslt"))
                {
                    StreamReader reader = new StreamReader(stream);
                    string content = await reader.ReadToEndAsync();
                    XmlDocument html2XamlXslDoc = new XmlDocument();
                    html2XamlXslDoc.LoadXml(content);
                    _html2XamlProcessor = new XsltProcessor(html2XamlXslDoc);
                }
            }

            // Apply XSLT to XML
            string xaml = _html2XamlProcessor.TransformToString(xhtmlDoc.FirstChild);
            return xaml;
        }

        /// <summary>
        /// Convert HTML to Rich textblock controls
        /// </summary>
        /// <param name="xhtml"></param>
        /// <returns></returns>
        private static async Task<string> ConvertHtmlToXamlRichTextBlock2([NotNull] string xhtml)
        {
            string xaml = "<?xml version=\"1.0\"?><RichTextBlock xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\">";

            try
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(xhtml);

                xaml = doc.DocumentNode.ChildNodes.Aggregate(xaml, (current, node) => current + GenerateBlockForTopNode(node));
            }
            catch (Exception)
            {
                // ignored
            }

            xaml += "</RichTextBlock>";

            return xaml;
        }

        private static string GenerateBlockForTopNode_old([NotNull] HtmlNode node)
        {
            string result;

            switch (node.Name)
            {
                case "h1":
                case "H1":
                    result = $"<Paragraph FontSize=\"30\">{CleanText(node.InnerText)}</Paragraph>";
                    break;
                case "h2":
                case "H2":
                    result = $"<Paragraph FontSize=\"24\">{CleanText(node.InnerText)}</Paragraph>";
                    break;
                case "h3":
                case "H3":
                    result = $"<Paragraph><Bold>{CleanText(node.InnerText)}</Bold></Paragraph>";
                    break;
                case "ul":
                case "UL":
                    result = $"<Paragraph Margin=\"20,0,0,0\"><LineBreak />{GenerateBlockForChildren(node)}</Paragraph>";
                    break;
                default:
                    //var text = GenerateBlockForChildren(node);
                    //if (!string.IsNullOrEmpty(text))
                    //{
                    //    result = String.Format("<Paragraph>{0}</Paragraph>", text);
                    //}
                    //else
                    //{
                    //    result = "";
                    //}
                    result = $"<Paragraph>{GenerateBlockForChildren(node)}</Paragraph>";
                    break;
            }

            return result;
        }

        private static string GenerateBlockForNode_old([NotNull] HtmlNode node)
        {
            string result;

            switch (node.Name)
            {
                case "div":
                    result = $"<Span>{GenerateBlockForChildren(node)}</Span>";
                    break;
                case "p":
                case "P":
                    result = $"<Span><LineBreak />{GenerateBlockForChildren(node)}<LineBreak /></Span>";
                    break;
                case "img":
                case "IMG":
                    {


                        //if (int.Parse(node.Attributes["width"].Value) > 500)
                        //    result = "<Span><InlineUIContainer><Image Style=\"{StaticResource RTImage}\" Width=\"500\" Source=\"" + node.Attributes["src"].Value + "\"></Image></InlineUIContainer></Span>";

                        //else
                        result = "<Span><InlineUIContainer><Image Style=\"{StaticResource RTImage}\" Source=\"" + node.Attributes["src"].Value + "\"></Image></InlineUIContainer></Span>";
                    }
                    break;
                case "a":
                case "A":
                    if (node.ChildNodes.Count == 1 && (node.FirstChild.Name == "img" || node.FirstChild.Name == "IMG"))
                        result = GenerateBlockForChildren(node);
                    else
                    {
                        if (node.ParentNode != null && (node.ParentNode.Name == "li" || node.ParentNode.Name == "LI"))
                            result = "<Span><InlineUIContainer><HyperlinkButton Style=\"{StaticResource RTLinkLI}\" NavigateUri=\"" + node.Attributes["href"].Value + "\">" + CleanText(node.InnerText) + "</HyperlinkButton></InlineUIContainer></Span>";
                        else if ((string.IsNullOrWhiteSpace(node.NextSibling?.InnerText)) && (string.IsNullOrWhiteSpace(node.PreviousSibling?.InnerText)))
                            result = "<Span><InlineUIContainer><HyperlinkButton Style=\"{StaticResource RTLinkOnly}\" NavigateUri=\"" + node.Attributes["href"].Value + "\">" + CleanText(node.InnerText) + "</HyperlinkButton></InlineUIContainer></Span>";
                        else
                            result = "<Span><InlineUIContainer><HyperlinkButton Style=\"{StaticResource RTLink}\" NavigateUri=\"" + node.Attributes["href"].Value + "\">" + CleanText(node.InnerText) + "</HyperlinkButton></InlineUIContainer></Span>";
                    }
                    break;
                case "li":
                case "LI":
                    result = "<Span><InlineUIContainer><Ellipse Style=\"{StaticResource RTBullet}\"/></InlineUIContainer>" + GenerateBlockForChildren(node) + "<LineBreak /></Span>";
                    break;
                case "b":
                case "B":
                    result = $"<Bold>{GenerateBlockForChildren(node)}</Bold>";
                    break;
                case "i":
                case "I":
                    result = $"<Italic>{GenerateBlockForChildren(node)}</Italic>";
                    break;
                case "u":
                case "U":
                    result = $"<Underline>{GenerateBlockForChildren(node)}</Underline>";
                    break;
                case "br":
                case "BR":
                    result = "<LineBreak />";
                    break;
                case "span":
                case "Span":
                    result = $"<Span>{GenerateBlockForChildren(node)}</Span>";
                    break;
                case "#text":
                    if (!string.IsNullOrWhiteSpace(node.InnerText))
                        result = CleanText(node.InnerText);
                    else
                        result = "";
                    break;
                default:
                    result = CleanText(node.InnerText);
                    break;
            }

            return result;
        }

        private static string GenerateBlockForChildren([NotNull] HtmlNode node)
        {
            string result = "";
            foreach (HtmlNode child in node.ChildNodes)
            {
                result += GenerateBlockForNode(child);
            }
            if (result == "")
                result = CleanText(node.InnerText);
            return result;
        }

        #endregion

    }
}
