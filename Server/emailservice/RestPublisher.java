package emailservice;

import org.apache.cxf.jaxrs.JAXRSServerFactoryBean;
public class RestPublisher {
    public static void main(String[] args){
        var bean = new JAXRSServerFactoryBean();
        var rest = new RestServiceDecorator(new QqEmailService());
        bean.setAddress(Config.restPublishAddr);
        bean.setServiceClass(rest.getClass());
        bean.setServiceBean(rest);
        bean.create();//发布一个服务
    }


}
