package emailservice;

public interface IEmailService {
    char sendEmail(String url, String payload);
    char sendEmailBatch(String[] url, String payload);
    char validateEmailAddress(String url);
}
