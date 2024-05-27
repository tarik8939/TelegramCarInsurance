# TelegramCarInsurance

### How to run
1. Run ngrok.exe from `/ngrok` folder
2. In command line write `ngrok config add-authtoken 2gsARD4RSBSNInHC2mxYCgdMKMX_7myhrBP5sDQqiVqFeeuju` to add authtoken to the default ngrok.yml configuration file.
3. In command line write `ngrok http http://localhost:5050`.
4. After you will see result like in image https://ibb.co/J5y4hSX, and copy Forwarding URL with `ngrok-free.app` in the end of address.
5. In browser follow the link `https://api.telegram.org/bot5914940880:AAETyY0Wj-Gbvr47NPFFWhX3sR1uLAZ_hxQ/setWebhook?url={url from previous step}` to set webhook. Exemple of final link `https://api.telegram.org/bot5914940880:AAETyY0Wj-Gbvr47NPFFWhX3sR1uLAZ_hxQ/setWebhook?url=https://0daa-188-163-9-29.ngrok-free.app`.6. And after this all you can start TelegramBot in VS
6. And after this all you can start TelegramBot in VS on 5050 port.

### Links 
- Bot - https://t.me/TarasCarInsuranceBot.
- Video demonstrating - https://youtu.be/2AnhB53CUU8.
- Bot workflow - https://ibb.co/PrcR6WR. I am not sure if this is correct workflow.

### How to improve 
- More flexible data validation and error handling.

##### If you have any questions or problems with ngrok, please contact me at E-mail: horilchaniytaras@gmail.com or Skype: live:.cid.77d6aebe9b034f79
