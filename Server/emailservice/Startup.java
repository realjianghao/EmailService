package emailservice;

public class Startup {
    public static void main(String[] args){
        Thread thread = new Thread(new Runnable() {
            @Override
            public void run() {
                RestPublisher.main(null);
            }
        });
        thread.start();

        SoapPublisher.main(null);
    }
}
