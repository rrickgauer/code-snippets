export class MessageEventDetail<T>
{
    public caller?: any;
    public data?: T;

    constructor(caller?: any, data?: T)
    {
        this.caller = caller;
        this.data = data;
    }
}


export class CustomMessage<T>
{
    private static readonly _body = document.querySelector('body') as HTMLBodyElement;

    public static invoke<T>(caller?: any, data?: T)
    {
        const eventName = this.prototype.constructor.name;
        const detail = new MessageEventDetail<T>(caller, data);

        const customEvent = new CustomEvent(eventName, {
            detail: detail,
            bubbles: true,
            cancelable: true,
        });

        this._body.dispatchEvent(customEvent);
    }

    public static addListener<T>(callback: (event: MessageEventDetail<T>) => void)
    {
        const body = document.querySelector('body');
        const eventName = this.prototype.constructor.name;

        this._body.addEventListener(eventName, (e: CustomEvent) =>
        {
            callback(e.detail);
        });
    }

}