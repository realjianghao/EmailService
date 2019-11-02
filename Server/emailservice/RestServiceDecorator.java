package emailservice;

import javax.jws.WebService;
import javax.ws.rs.*;
import javax.ws.rs.core.MediaType;
import javax.ws.rs.core.Response;
import java.util.List;

@WebService
@Path("/service")
public class RestServiceDecorator implements IEmailService {
    private IEmailService emailService;

    public RestServiceDecorator(IEmailService emailService) {
        this.emailService = emailService;
    }

    public RestServiceDecorator(){
        this(new QqEmailService());
    }

    @Override
    @GET
    @Path("/sendemail")
    @Produces("text/plain")
    public char sendEmail(@QueryParam("url") String url,@QueryParam("payload") String payload) {
        var res =  emailService.sendEmail(url,payload);
        return res;
        //return Response.ok(res.toString(), MediaType.APPLICATION_XML).build();
    }

    @Override
    public char sendEmailBatch(String[] url, String payload) {
        return 0;

    }

    @Override
    @GET
    @Path("/validate")
    @Produces("text/plain")
    public char validateEmailAddress(@QueryParam("url") String url) {
        return emailService.validateEmailAddress(url);
    }
}
