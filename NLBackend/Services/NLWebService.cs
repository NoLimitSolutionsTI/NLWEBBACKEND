using Microsoft.Extensions.Options;
using MongoDB.Driver;
using NLBackend.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NLBackend.Services
{
    public class NLWebService
    {
        private readonly IMongoCollection<Contacts> _nlCollection;
        private readonly IEmailSender _email;
        private readonly string? _adminBcc;

        public NLWebService(
            IOptions<NLWebDatabaseSettings> nlWebDatabaseSettings,
            IOptions<SmtpSettings> smtpOptions,
            IEmailSender email)

        {
            var mongoClient = new MongoClient(
                nlWebDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                nlWebDatabaseSettings.Value.DatabaseName);

            _nlCollection = mongoDatabase.GetCollection<Contacts>(
                nlWebDatabaseSettings.Value.NLCollectionName);

            // SMTP CONFIG
            _email = email;
            _adminBcc = smtpOptions.Value.ToAddressOnCreate;
        }

        public async Task<List<Contacts>> GetContactsAsync() =>
            await _nlCollection.Find(_ => true).ToListAsync();

        public async Task<Contacts?> GetContactAsync(string id) =>
            await _nlCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync (Contacts newContact)
        {
            await _nlCollection.InsertOneAsync(newContact);

            // Si no viene email, no se envia la notificación
            if (string.IsNullOrWhiteSpace(newContact.Email))
                return;

            try
            {
                var names = string.IsNullOrWhiteSpace(newContact.FullName) ? "Hola" : newContact.FullName;
                var subject = "¡Hemos recibido tu mensaje!";
                var body = $@"
                    <div style='font-family:Arial,Helvetica,sans-serif;line-height:1.5'>
                      <h2 style='margin:0 0 12px'>Gracias por contactarnos</h2>
                      <p>{System.Net.WebUtility.HtmlEncode(names)}, recibimos tu solicitud. Te responderemos a la brevedad.</p>
                      <hr style='border:none;border-top:1px solid #eee;margin:16px 0' />
                      <p style='margin:0 0 8px'><b>Resumen</b></p>
                      <ul style='padding-left:18px;margin:0'>
                        <li><b>Nombre:</b> {System.Net.WebUtility.HtmlEncode(newContact.FullName)}</li>
                        <li><b>Email:</b> {System.Net.WebUtility.HtmlEncode(newContact.Email)}</li>
                        <li><b>Teléfono:</b> {System.Net.WebUtility.HtmlEncode(newContact.Telephone)}</li>
                      </ul>
                      <p style='color:#777;margin-top:16px'>Este es un mensaje automático.</p>
                    </div>";

                await _email.SendAsync(newContact.Email.Trim(), subject, body, _adminBcc);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Email] Error enviando al contacto: {ex.Message}");
            }
        }
    }
}
