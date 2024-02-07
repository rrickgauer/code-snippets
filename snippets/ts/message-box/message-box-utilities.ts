export type ShowMessageBoxData = {
    message: string;
    title?: string;
}



export class MessageBoxUtilities
{
    public static showStandard = (message: ShowMessageBoxData) => MessageBoxUtilities.show(message, MessageBoxType.STANDARD);
    public static showSuccess = (message: ShowMessageBoxData) => MessageBoxUtilities.show(message, MessageBoxType.SUCCESS);
    public static showError = (message: ShowMessageBoxData) => MessageBoxUtilities.show(message, MessageBoxType.ERROR);
    
    public static show = (message: ShowMessageBoxData, messageType: MessageBoxType): MessageBoxBase =>
    {
        switch (messageType)
        {
            case MessageBoxType.STANDARD:
                return new MessageBoxStandard(message.message, message.title).show();

            case MessageBoxType.SUCCESS:
                return new MessageBoxSucccess(message.message, message.title).show();

            case MessageBoxType.ERROR:
                return new MessageBoxError(message.message, message.title).show();
        }

        throw new Error('Invalid message box type');
    }
}