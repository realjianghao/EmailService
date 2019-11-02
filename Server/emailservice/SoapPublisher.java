package emailservice;

import javax.xml.ws.Endpoint;

public class SoapPublisher {
    public static void main(String[] args) {
        String address = "http://localhost:8080/EmailService";
        //access:http://localhost:8080/EmailService?wsdl

        Object implementor = new SoapServiceDecorator(new QqEmailService());

        Endpoint.publish(address, implementor);

    }
}
