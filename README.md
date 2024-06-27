# TelegramCarInsurance

#### Important: this is a branch with InlineKeyboardButton, if you want KeyboardButton, switch the branch to `master'.

### System requirements
1. .Net 7

### How to run
1. Run ngrok.exe from `TelegramCarInsurance/ngrok` folder
2. In command line write `ngrok config add-authtoken 2gsARD4RSBSNInHC2mxYCgdMKMX_7myhrBP5sDQqiVqFeeuju` to add authtoken to the default ngrok.yml configuration file.
3. In command line write `ngrok http http://localhost:5050`.
4. After you will see result like in image https://ibb.co/J5y4hSX, and copy Forwarding URL with `ngrok-free.app` in the end of address.
5. In browser follow the link `https://api.telegram.org/bot5914940880:AAETyY0Wj-Gbvr47NPFFWhX3sR1uLAZ_hxQ/setWebhook?url={url from previous step}` to set webhook. Exemple of final link `https://api.telegram.org/bot5914940880:AAETyY0Wj-Gbvr47NPFFWhX3sR1uLAZ_hxQ/setWebhook?url=https://0daa-188-163-9-29.ngrok-free.app`.
6. And after this all you can start TelegramBot in VS on 5050 port.
7. To extract data use prepared fake documents in `TelegramCarInsurance/fake data` folder.

##### Important: in appsettings.json set your own token(with Credit balance)  in OpenAi_API_Key because in public repository OpenAi_API_Key will be automatically disabled. Or write to me and I will send you active key.

### How to improve
- Improve inline button menu.
- More flexible error handling.

### Links 
- Bot - https://t.me/TarasCarInsuranceBot.
- Video demonstrating - https://youtu.be/7_MUBC4ifgQ.
- Bot workflow - https://ibb.co/PrcR6WR. I am not sure if this is correct workflow.

##### If you have any questions, problems with ngrok or API keys, please contact me at E-mail: horilchaniytaras@gmail.com or Skype: live:.cid.77d6aebe9b034f79