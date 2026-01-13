using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Infrastructure.ExternalServices
{
    public class GmailClient
    {
        private readonly ILogger<GmailClient> _logger;
        private readonly IConfiguration _config;
        private readonly string _brotherEmail;

        public GmailClient(ILogger<GmailClient> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
            _brotherEmail = _config["Gmail:BrotherEmail"]!;
        }

        public async Task SendOrderNotificationAsync(string orderId, decimal totalAmount, string customerName)
        {
            try
            {
                var credential = await GetCredentialAsync();

                using var service = new GmailService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "MinashBackend",
                });

                string logoUrl = "https://i.postimg.cc/MpDBdwcN/descarga.png";
                string subject = $"✧ ¡Venta Confirmada! - {customerName}";

                string bodyHtml = $@"
                <div style='font-family: Sans-serif; background-color: #f4f4f4; text-align: center;'>
                    <div style='max-width: 600px; margin: auto; background-color: #ffffff; border-radius: 8px; overflow: hidden; box-shadow: 0 4px 10px rgba(0,0,0,0.1);'>
                        
                        <div style='background-color: #000000; padding: 20px;'>
                            <img src='{logoUrl}' alt='Minash Logo' style='width: 150px;'>
                        </div>

                        <div style='padding: 20px; text-align: left;'>
                            <h1 style='color: #333; font-size: 24px; margin-bottom: 10px;'>¡Gran trabajo! Tenemos una nueva venta </h1>
                            <p style='color: #666; font-size: 16px; line-height: 1.5;'>
                                Hola, se ha registrado un nuevo pedido en el sistema. Es momento de preparar las tintas y los bastidores para una nueva personalización.
                            </p>
                            
                            <hr style='border: 0; border-top: 1px solid #eee; margin: 25px 0;'>

                            <table style='width: 100%; border-collapse: collapse;'>
                                <tr>
                                    <td style='padding: 8px 0; color: #888;'>ID de Orden:</td>
                                    <td style='padding: 8px 0; font-weight: bold; text-align: right;'>#{orderId}</td>
                                </tr>
                                <tr>
                                    <td style='padding: 8px 0; color: #888;'>Cliente:</td>
                                    <td style='padding: 8px 0; font-weight: bold; text-align: right;'>{customerName}</td>
                                </tr>
                                <tr>
                                    <td style='padding: 8px 0; color: #888;'>Total Recibido:</td>
                                    <td style='padding: 8px 0; font-weight: bold; text-align: right; color: #28a745; font-size: 18px;'>${totalAmount}</td>
                                </tr>
                                <tr>
                                    <td style='padding: 8px 0; color: #888;'>Fecha:</td>
                                    <td style='padding: 8px 0; font-weight: bold; text-align: right;'>{DateTime.Now:dd/MM/yyyy HH:mm}</td>
                                </tr>
                            </table>

                            <div style='margin-top: 35px; text-align: center;'>
                                <a href='https://tudominio.com/admin/orders/{orderId}' 
                                   style='background-color: #000; color: #fff; padding: 15px 30px; text-decoration: none; border-radius: 5px; font-weight: bold; display: inline-block;'>
                                   GESTIONAR PRODUCCIÓN
                                </a>
                            </div>
                        </div>

                        <div style='background-color: #fafafa; padding: 20px; text-align: center; color: #aaa; font-size: 12px;'>
                            Este es un mensaje automático enviado por el sistema de gestión de Minash.<br>
                            © {DateTime.Now.Year} Serigrafía & Estilo.
                        </div>
                    </div>
                </div>"; 

                var message = CreateEmail(_brotherEmail, subject, bodyHtml);

                await service.Users.Messages.Send(message, "me").ExecuteAsync();

                _logger.LogInformation($"Correo de notificación enviado correctamente para la orden {orderId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error enviando correo Gmail para la orden {orderId}");
            }
        }

        private async Task<UserCredential> GetCredentialAsync()
        {
            var clientId = _config["Gmail:ClientId"];
            var clientSecret = _config["Gmail:ClientSecret"];
            var refreshToken = _config["Gmail:RefreshToken"];

            var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = new ClientSecrets
                {
                    ClientId = clientId,
                    ClientSecret = clientSecret
                },
                Scopes = new[] { GmailService.Scope.GmailSend }
            });

            var token = new TokenResponse { RefreshToken = refreshToken };

            return new UserCredential(flow, "user", token);
        }

        private Message CreateEmail(string to, string subject, string bodyHtml)
        {
            var senderEmail = _config["Gmail:SenderEmail"];
            var senderName = _config["Gmail:SenderName"];

            string msg = $"To: {to}\r\n" +
                         $"Subject: =?utf-8?B?{Convert.ToBase64String(Encoding.UTF8.GetBytes(subject))}?=\r\n" +
                         $"From: \"{senderName}\" <{senderEmail}>\r\n" +
                         "Content-Type: text/html; charset=utf-8\r\n" +
                         "\r\n" +
                         bodyHtml;

            return new Message
            {
                Raw = Base64UrlEncode(msg)
            };
        }

        private static string Base64UrlEncode(string input)
        {
            var inputBytes = Encoding.UTF8.GetBytes(input);
            return Convert.ToBase64String(inputBytes)
                .Replace('+', '-')
                .Replace('/', '_')
                .Replace("=", "");
        }
        public async Task SendOrderCompletedNotificationAsync(string customerEmail, string customerName, string orderId)
        {
            try
            {
                var credential = await GetCredentialAsync();

                using var service = new GmailService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "MinashBackend",
                });

                string logoUrl = "https://i.postimg.cc/MpDBdwcN/descarga.png";
                string subject = "✧ ¡Tu pedido está listo para retiro! - Minash";

                // Aquí puedes configurar el punto de retiro según necesites
                string pickupLocation = "Calle Falsa 123, Ciudad";
                string pickupSchedule = "Lunes a Viernes de 10:00 a 18:00hs";

                string bodyHtml = $@"
        <div style='font-family: Sans-serif; background-color: #f4f4f4; text-align: center;'>
            <div style='max-width: 600px; margin: auto; background-color: #ffffff; border-radius: 8px; overflow: hidden; box-shadow: 0 4px 10px rgba(0,0,0,0.1);'>
                
                <div style='background-color: #000000; padding: 20px;'>
                    <img src='{logoUrl}' alt='Minash Logo' style='width: 150px;'>
                </div>

                <div style='padding: 20px ; text-align: left;'>
                    <h1 style='color: #333; font-size: 24px; margin-bottom: 10px;'>¡Hola, {customerName}!</h1>
                    <p style='color: #666; font-size: 16px; line-height: 1.5;'>
                        Nos alegra informarte que el trabajo de producción de tu pedido <b>#{orderId}</b> ha finalizado con éxito. ¡Ya está listo para que lo disfrutes!
                    </p>
                    
                    <div style='background-color: #f8f9fa; border-left: 4px solid #C9A86A; padding: 15px; margin: 25px 0;'>
                        <p style='margin: 0 0 5px 0; color: #888; font-size: 12px; font-weight: bold; text-transform: uppercase;'>Informacion de retiro:</p>
                        <p style='margin: 0; color: #333; font-style: italic; line-height: 1.4;'>
                            <p><b>📍 Lugar:</b> {pickupLocation}</p>
                            <p><b>⏰ Horarios:</b> {pickupSchedule}</p>
                        </p>
                    </div>

                    <p style='color: #666; font-size: 14px;'>
                        Gracias por confiar en <b>Minash</b> para tus personalizaciones. Si tienes alguna duda sobre el retiro, puedes contactarnos por WhatsApp.
                    </p>

                    <hr style='border: 0; border-top: 1px solid #eee; margin: 25px 0;'>

                    <div style='text-align: center;'>
                        <p style='color: #888; font-size: 14px; margin-bottom: 20px;'>¡Te esperamos!</p>
                        <a href='https://wa.me/3513501278' 
                           style='background-color: #000; color: #fff; padding: 15px 30px; text-decoration: none; border-radius: 5px; font-weight: bold; display: inline-block;'>
                            CONTACTAR POR WHATSAPP
                        </a>
                    </div>
                </div>

                <div style='background-color: #fafafa; padding: 20px; text-align: center; color: #aaa; font-size: 12px;'>
                     Este es un mensaje automático enviado por el sistema de gestión de Minash..<br>
                    © {DateTime.Now.Year} Serigrafía & Estilo.
                </div>
            </div>
        </div>";

                var message = CreateEmail(customerEmail, subject, bodyHtml);
                await service.Users.Messages.Send(message, "me").ExecuteAsync();

                _logger.LogInformation($"Correo de pedido completado enviado a {customerEmail} (Orden {orderId})");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error enviando correo de retiro para la orden {orderId}");
            }
        }

        public async Task SendPaymentConfirmationToCustomerAsync(string customerEmail, string customerName, string orderId, decimal amount, string orderUrl)
        {
            try
            {
                var credential = await GetCredentialAsync();

                using var service = new GmailService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "MinashBackend",
                });

                string logoUrl = "https://i.postimg.cc/MpDBdwcN/descarga.png";
                string subject = $"✧ ¡Pago recibido - Orden #{orderId}";

                string bodyHtml = $@"
        <div style='font-family: Sans-serif; background-color: #f4f4f4; text-align: center;'>
            <div style='max-width: 600px; margin: auto; background-color: #ffffff; border-radius: 8px; overflow: hidden; box-shadow: 0 4px 10px rgba(0,0,0,0.1);'>
                
                <div style='background-color: #000000; padding: 20px;'>
                    <img src='{logoUrl}' alt='Minash Logo' style='width: 150px;'>
                </div>

                <div style='padding: 20px; text-align: left;'>
                    <h1 style='color: #28a745; font-size: 24px; margin-bottom: 10px; text-align: center;'>¡Pago Exitoso!</h1>
                    
                    <p style='color: #333; font-size: 16px; line-height: 1.5; margin-bottom: 20px;'>
                        Hola <b>{customerName}</b>, traemos buenas noticias.
                    </p>

                    <p style='color: #666; font-size: 15px; line-height: 1.6;'>
                        Tu pago de <b style='color:#333'>${amount:N2}</b> ha impactado correctamente en nuestro sistema. 
                        Queremos transmitirte tranquilidad: <b>tu pedido ya está confirmado.</b>
                    </p>

                    <div style='background-color: #f8f9fa; border-left: 4px solid #C9A86A; padding: 15px; margin: 25px 0;'>
                        <p style='margin: 0 0 5px 0; color: #888; font-size: 12px; font-weight: bold; text-transform: uppercase;'>Estado actual:</p>
                          <p style='margin: 0; color: #333; font-style: italic; line-height: 1.4;'>
                            El sistema ha notificado automáticamente a nuestro equipo técnico. 
                            Ya tienen la orden de preparar los materiales y comenzar con la producción de tu diseño.
                          </p>
                    </div>

                    <p style='color: #666; font-size: 14px;'>
                        Puedes ver el detalle de tu compra y seguir el estado en tiempo real haciendo clic en el siguiente botón:
                    </p>

                    <div style='margin-top: 35px; text-align: center;'>
                        <a href='{orderUrl}' 
                           style='background-color: #000; color: #fff; padding: 15px 30px; text-decoration: none; border-radius: 5px; font-weight: bold; display: inline-block; box-shadow: 0 2px 5px rgba(0,0,0,0.2);'>
                            VER MI PEDIDO
                        </a>
                    </div>
                </div>

                <div style='background-color: #fafafa; padding: 20px; text-align: center; color: #aaa; font-size: 12px;'>
                    Este es un comprobante automático de Minash.<br>
                    © {DateTime.Now.Year} Serigrafía & Estilo.
                </div>
            </div>
        </div>";

                var message = CreateEmail(customerEmail, subject, bodyHtml);
                await service.Users.Messages.Send(message, "me").ExecuteAsync();

                _logger.LogInformation($"Correo de confirmación de pago enviado al cliente {customerEmail} (Orden {orderId})");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error enviando correo de confirmación de pago al cliente para la orden {orderId}");
            }
        }

        public async Task SendBudgetAlertAsync(string clientName, string clientEmail, string phone, string category, string description)
        {
            try
            {
                var credential = await GetCredentialAsync();

                using var service = new GmailService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "MinashBackend",
                });

                string logoUrl = "https://i.postimg.cc/MpDBdwcN/descarga.png";
                string subject = $"✧ Solicitud de Presupuesto - {clientName} ({category})";

                string bodyHtml = $@"
        <div style='font-family: Sans-serif; background-color: #f4f4f4; text-align: center; padding: 10px;'>
            <div style='max-width: 600px; margin: auto; background-color: #ffffff; border-radius: 8px; overflow: hidden; box-shadow: 0 4px 10px rgba(0,0,0,0.1);'>
                
                <div style='background-color: #000000; padding: 20px;'>
                    <img src='{logoUrl}' alt='Minash Logo' style='width: 150px;'>
                </div>

                <div style='padding: 20px; text-align: left;'>
                    <h1 style='color: #333; font-size: 24px; margin-bottom: 10px;'>¡Nueva consulta técnica! 🚀</h1>
                    <p style='color: #666; font-size: 16px; line-height: 1.5;'>
                        Hola, se ha recibido una nueva solicitud de presupuesto desde la web. Un cliente está interesado en realizar un trabajo de <b>{category}</b>.
                    </p>
                    
                    <hr style='border: 0; border-top: 1px solid #eee; margin: 25px 0;'>

                    <table style='width: 100%; border-collapse: collapse;'>
                        <tr>
                            <td style='padding: 8px 0; color: #888;'>Cliente:</td>
                            <td style='padding: 8px 0; font-weight: bold; text-align: right;'>{clientName}</td>
                        </tr>
                        <tr>
                            <td style='padding: 8px 0; color: #888;'>Email:</td>
                            <td style='padding: 8px 0; font-weight: bold; text-align: right;'>{clientEmail}</td>
                        </tr>
                        <tr>
                            <td style='padding: 8px 0; color: #888;'>Teléfono:</td>
                            <td style='padding: 8px 0; font-weight: bold; text-align: right;'>{phone}</td>
                        </tr>
                        <tr>
                            <td style='padding: 8px 0; color: #888;'>Categoría:</td>
                            <td style='padding: 8px 0; font-weight: bold; text-align: right; color: #C9A86A;'>{category}</td>
                        </tr>
                        <tr>
                            <td style='padding: 8px 0; color: #888;'>Fecha:</td>
                            <td style='padding: 8px 0; font-weight: bold; text-align: right;'>{DateTime.Now:dd/MM/yyyy HH:mm}</td>
                        </tr>
                    </table>

                    <div style='background-color: #f8f9fa; border-left: 4px solid #C9A86A; padding: 15px; margin: 25px 0;'>
                        <p style='margin: 0 0 5px 0; color: #888; font-size: 12px; font-weight: bold; text-transform: uppercase;'>Descripción del Proyecto:</p>
                        <p style='margin: 0; color: #333; font-style: italic; line-height: 1.4;'>
                            ""{description}""
                        </p>
                    </div>

                    <div style='margin-top: 35px; text-align: center;'>
                        <a href='mailto:{clientEmail}?subject=Presupuesto Minash - {category}' 
                           style='background-color: #000; color: #fff; padding: 15px 30px; text-decoration: none; border-radius: 5px; font-weight: bold; display: inline-block;'>
                            RESPONDER AL CLIENTE
                        </a>
                    </div>
                </div>

                <div style='background-color: #fafafa; padding: 20px; text-align: center; color: #aaa; font-size: 12px;'>
                    Este es un mensaje automático enviado por el sistema de gestión de Minash.<br>
                    <br/>
                    © {DateTime.Now.Year} Serigrafía & Estilo.
                </div>
            </div>
        </div>";

                var message = CreateEmail(_brotherEmail, subject, bodyHtml);

                await service.Users.Messages.Send(message, "me").ExecuteAsync();

                _logger.LogInformation($"Solicitud de presupuesto de {clientName} enviada a administración.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error enviando correo de presupuesto.");
                throw;
            }
        }
    }
}