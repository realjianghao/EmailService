package emailservice;

import javax.mail.*;
import javax.mail.internet.InternetAddress;
import javax.mail.internet.MimeBodyPart;
import javax.mail.internet.MimeMessage;
import javax.mail.internet.MimeMultipart;
import java.util.Date;
import java.util.Properties;
import java.util.regex.Pattern;

public class QqEmailService implements IEmailService {
    //发送邮箱
    //private String fromEmail =
    private String emailType = "smtp.qq.com";

    //QQ邮箱授权码 开通POP3/SMTP服务 的授权码
    private String emailAuthPassword = "cfwspiihkuzdicib";
    //"dhufhqwmvclvigbd";

    private boolean debug = true;

    private Session newSession() {
        Properties props = new Properties();
        props.put("mail.smtp.host", emailType);//发件人使用发邮件的电子信箱服务器
        props.put("mail.password", emailAuthPassword);
        props.put("mail.transport.protocol", "smtp");
        props.setProperty("mail.debug", Boolean.toString(debug));
        props.put("mail.smtp.auth", "true"); //这样才能通过验证

        //下面这三句很重要，如果没有加入进去，qq邮箱会验证不成功，一直报503错误
        props.put("mail.smtp.socketFactory.class", "javax.net.ssl.SSLSocketFactory");
        props.put("mail.smtp.port", "465");
        props.put("mail.smtp.socketFactory.port", "465");


        //获得默认的session对象
        Session mailSession = Session.getInstance(props);
        mailSession.setDebug(debug);
        return mailSession;
    }

    private MimeMessage newMimeMessage(String url, Session mailSession, String body) throws Exception {
        MimeMessage mimeMessage = new MimeMessage(mailSession);

        InternetAddress from = new InternetAddress(Config.qqEmailAddr);
        mimeMessage.setFrom(from);
        InternetAddress to = new InternetAddress(url); //设置收件人地址并规定其类型
        mimeMessage.setRecipient(Message.RecipientType.TO, to);

        mimeMessage.setSentDate(new Date());    //设置发信时间
        //mimeMessage.setSubject(subject);        //设置主题
        mimeMessage.setText(body);                //设置 正文

        //给消息对象设置内容
        BodyPart bodyPart = new MimeBodyPart();                    //新建一个存放信件内容的BodyPart对象
        bodyPart.setContent(body, "text/html;charset= utf-8");    //设置 发送邮件内容为HTML类型,并为中文编码

        Multipart multipart = new MimeMultipart();
        multipart.addBodyPart(bodyPart);

        mimeMessage.setContent(multipart);
        mimeMessage.saveChanges();
        return mimeMessage;
    }

    private void sendEmail(Session mailSession, MimeMessage mimeMessage) throws Exception {
        //发送消息
        Transport transport = mailSession.getTransport("smtp");
        transport.connect(emailType, Config.qqEmailAddr, emailAuthPassword);//发邮件人帐户密码,此外是我的帐户密码，使用时请修改news.properties中的值 。
        transport.sendMessage(mimeMessage, mimeMessage.getAllRecipients());
        transport.close();
    }

    @Override
    public char sendEmail(String url, String payload) {

        String body = payload; //正文内容
        try {
            var mailSession = newSession();
            var mimeMessage = newMimeMessage(url, mailSession, body);
            sendEmail(mailSession, mimeMessage);

            if (debug) {
                mimeMessage.writeTo(System.out);//保存消息 并在控制台 显示消息对象中属性信息
                System.out.println("邮件已成功发送到:" + url);
            }
            return 'Y';
        } catch (Exception e) {
            e.printStackTrace();
            return 'N';
        }
    }

    @Override
    public char sendEmailBatch(String[] url, String payload) {

        for (var i : url) {
            char res = sendEmail(i, payload);
            if (res == 'N') {
                return 'N';
            }
        }
        return 'Y';
    }

    @Override
    public char validateEmailAddress(String url) {
        //邮件地址命名规则为“名称@域名”
        //名称：[a-zA-Z0-9_-]+
        //域名：[a-zA-Z0-9_-]+(\.[a-zA-Z0-9_-]+)+
        final String patternStr = "^[a-zA-Z0-9_-]+@[a-zA-Z0-9_-]+(\\.[a-zA-Z0-9_-]+)+$";
        //Pattern pattern = Pattern.compile(patternStr);
        var res = Pattern.matches(patternStr, url);
        //var res = pattern.matcher(url);
        return res ? 'Y' : 'N';
    }
}
