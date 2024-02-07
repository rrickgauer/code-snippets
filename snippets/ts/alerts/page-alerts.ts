export enum AlertTypes 
{
    PRIMARY   = 'alert-primary',
    SECONDARY = 'alert-secondary',
    SUCCESS   = 'alert-success',
    DANGER    = 'alert-danger',
    WARNING   = 'alert-warning',
    INFO      = 'alert-info',
    LIGHT     = 'alert-light',
    DARK      = 'alert-dark',
}


//#region Base class


export class AlertPageTopBase
{

    public static getPageTopContainer = (): HTMLDivElement =>
    {
        return document.querySelector('#alerts-page-top') as HTMLDivElement;
    }

    protected _message: string;
    protected _alertType: AlertTypes;
    protected _container: HTMLDivElement;

    constructor(container: HTMLDivElement, message: string, alertType: AlertTypes)
    {
        this._message = message;
        this._alertType = alertType;
        this._container = container;
    }

    public show = () =>
    {
        this._container.innerHTML = this._getHtml();
    }

    protected _getHtml = (): string =>
    {

        let html = `

            <div class="alert ${this._alertType} alert-dismissible show" role="alert">
                <div>${this._message}</div>
                <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
            </div>

        `;

        return html;
    }
}

//#endregion


export class AlertPageTopSuccess extends AlertPageTopBase
{
    constructor(container: HTMLDivElement, message: string)
    {
        super(container, message, AlertTypes.SUCCESS);
    }
}


export class AlertPageTopDanger extends AlertPageTopBase
{
    constructor(container: HTMLDivElement, message: string)
    {
        super(container, message, AlertTypes.DANGER);
    }
}



