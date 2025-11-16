export * from './customers.service';
import { CustomersService } from './customers.service';
export * from './orders.service';
import { OrdersService } from './orders.service';
export * from './status.service';
import { StatusService } from './status.service';
export const APIS = [CustomersService, OrdersService, StatusService];
