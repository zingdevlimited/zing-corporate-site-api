namespace corporate_site_api.Models
{
    public class Submission {

        public SubmissionContact Contact {get;set;}
        public SubmissionMessage Message {get;set;}
        public string Token;

        public HubspotContact CreateContact(long assignedUser) {
            return new HubspotContact(Contact,assignedUser);
        }

        public HubspotNote CreateNote(long contactId) {
            return new HubspotNote(Message,contactId);
        }

    }

    public class SubmissionContact {
        public string FirstName {get;set;}
        public string LastName {get;set;}
        public string Email {get;set;}
    }

    public class SubmissionMessage {
        public string Subject {get;set;}
        public string Message {get;set;}
    }

}