package emailservice;

import emailservice.IEmailService;

import javax.jws.WebMethod;
import javax.jws.WebService;

@WebService
public class SoapServiceDecorator implements IEmailService {

    private IEmailService emailSender;

    public SoapServiceDecorator(){

    }

    public SoapServiceDecorator(IEmailService emailSender){
        this.emailSender = emailSender;
    }

    @Override
    @WebMethod
    public char sendEmail(String url, String payload) {
        return emailSender.sendEmail(url, payload);
    }

    @WebMethod
    @Override
    public char sendEmailBatch(String[] url, String payload) {

        return emailSender.sendEmailBatch(url, payload);
    }

    @WebMethod
    @Override
    public char validateEmailAddress(String url) {

        return emailSender.validateEmailAddress(url);
    }
}
