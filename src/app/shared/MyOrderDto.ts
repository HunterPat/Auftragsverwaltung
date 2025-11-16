import { OrderDto } from "../swagger";

export class MyOrderDto implements OrderDto{ 
    isSelected: boolean = false;
    id?: number;
    inputDate?: string;
    deliveryDate?: string;
    billCreatedDate?: string;
    paymentDate?: string;
    documentNr?: number;
    brutto?: number;
    netto?: number;
    tax?: number;
    customerName?: string | null;
    bill?: number;
    po?: string | null;
    status?: string | null;
    uid?: string | null;
}