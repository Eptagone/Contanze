# ConstanzeBot

A bot that uses the Safone ChatGPT API to talk to you.


## How to Run: Long Polling

You just need to specify your **bot token** and your payment **provider token** in the `ShopBotNET.AppService` project.

The **provider token** allows your bot to use the payment API. If you don't already have it, you can get one for testing by creating an account on [stripe](https://stripe.com/) and [enabling TEST mode](https://stripe.com/). Then [go to BotFather and get your token](https://core.telegram.org/bots/payments#getting-a-token).

Your `secrets.json` or `appsettings.json` should look like the following code:

```JSON
{
    "ConnectionStrings": {
        "Default": "Server=(localdb)\\mssqllocaldb;Database=ConstanzeBot;Trusted_Connection=True;MultipleActiveResultSets=true"
    },
    "Constanze": {
        "BotToken": "123456:ABC-DEF1234ghIkl-zyx57W2v1u123ew11"
    }
}
```

You can also use enviroment variables instead:

| Env                        | Description                                                                                      |
| :------------------------- | :----------------------------------------------------------------------------------------------- |
| ConnectionStrings__Default | The connection string to your SQL database.                                                      |
| Constanze\_\_BotToken      | Your bot token provided by [@BotFather](https://t.me/BotFather).                                 |
| Constanze\_\_MaxMessages   | Optional. The maximum number of messages that can be sent in a conversation. Default value is 20 |
