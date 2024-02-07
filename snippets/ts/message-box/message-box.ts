import { Modal } from "bootstrap";
import { NativeEvents } from "../../constants/native-events";
import { BootstrapModalEvents } from "../../constants/bootstrap-constants";


export enum MessageBoxType
{
    STANDARD,
    SUCCESS,
    ERROR,
    CONFIRM,
}


export abstract class MessageBoxBase
{
    protected title?: string;
    protected message: string;

    public abstract messageBoxType: MessageBoxType;
    public abstract element: HTMLDivElement;
    protected abstract defaultTitle: string;


    public get modal(): Modal
    {
        return Modal.getOrCreateInstance(this.element);
    }

    public get elementTitle(): HTMLElement
    {
        return this.element.querySelector('.modal-title');
    }

    public get elementContent(): HTMLElement
    {
        return this.element.querySelector('.modal-body-content');
    }

    constructor(message: string, title?: string)
    {
        this.message = message;
        this.title = title;
    }


    public show = () =>
    {
        if (this.element == null)
        {
            alert(`MessageBox: ${this.message ?? ''}`);
            console.error(`The container element has not been set.`);
            return this;
        }

        this.elementTitle.innerText = this.title ?? this.defaultTitle;
        this.elementContent.innerHTML = this.message ?? '';

        this.modal.show();

        return this;
    }

    public close = () =>
    {
        this.modal.hide();
        return this;
    }

    protected setTitle = (title?: string) => 
    {
        this.title = title ?? this.defaultTitle;
    }

}

export class MessageBoxStandard extends MessageBoxBase
{
    public readonly messageBoxType = MessageBoxType.STANDARD;
    protected defaultTitle = "Details";

    public get element()
    {
        return document.querySelector('#message-box-standard') as HTMLDivElement;
    }

    constructor(message: string, title?: string)
    {
        super(message, title);
    }


}


export class MessageBoxSucccess extends MessageBoxBase
{
    public readonly messageBoxType = MessageBoxType.SUCCESS;
    protected defaultTitle = "Success";

    public get element()
    {
        return document.querySelector('#message-box-success') as HTMLDivElement;
    }

    constructor(message: string, title?: string)
    {
        super(message, title);
    }
}

export class MessageBoxError extends MessageBoxBase
{
    public readonly messageBoxType = MessageBoxType.ERROR;
    protected defaultTitle = "Error";

    public get element()
    {
        return document.querySelector('#message-box-error') as HTMLDivElement;
    }

    constructor(message: string, title?: string)
    {
        super(message, title);
    }
}



export type MessageBoxConfirmArgs = {
    onSuccess?: () => void;
    //onCancel?: () => void;
}


export class MessageBoxConfirm extends MessageBoxBase
{
    public readonly messageBoxType = MessageBoxType.ERROR;
    protected defaultTitle = "Confirm";

    private readonly _defaultConfirmButtonText = "Confirm";
    private readonly _btnConfirm: HTMLButtonElement;

    private onSuccess?: () => void;

    public get element()
    {
        return document.querySelector('#message-box-confirm') as HTMLDivElement;
    }

    constructor(message: string, confirmButtonText?: string, title?: string)
    {
        super(message, title);

        this._btnConfirm = this.element.querySelector('[data-js-confirm]') as HTMLButtonElement;
        this._btnConfirm.innerText = confirmButtonText ?? this._defaultConfirmButtonText;

        this.element.addEventListener(BootstrapModalEvents.Hidden, () =>
        {
            this._btnConfirm.removeEventListener(NativeEvents.Click, this.clickHandler);
        });
    }

    public confirm = (args: MessageBoxConfirmArgs) =>
    {
        this.onSuccess = args.onSuccess;
        this.show();
        this._btnConfirm.addEventListener(NativeEvents.Click, this.clickHandler);
    }

    private clickHandler = (e) =>
    {
        e.preventDefault();
        this.onSuccess();

        // Remove the click event listener
        this._btnConfirm.removeEventListener(NativeEvents.Click, this.clickHandler);

        this.close();
    };


}





