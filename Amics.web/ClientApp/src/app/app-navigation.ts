import { ComponentType } from "./pages/models/componentType";

export const navigation = [
  {
    text: 'Home',
    path: '/home',
    icon: 'home',
  },
  {
    text: 'Inventory',
    icon: 'folder',
    items: [
      {
        text: 'Part Master & BOM',
        path: '/profile',
        title: 'Part Master',
        type: ComponentType.PartMaster
      },
      {
        text: 'Increase Inventory',
        path: '/increase-inventory',
        title: 'Increase Inventory',
        type: ComponentType.IncreaseInventory
      },
      {
        text: 'Adjust Inventory',
        path: '/adjustInventory',
        title: 'Adjust Inventory',
        type: ComponentType.Profile
      },
      {
        text: 'Change Location',
        path: '/changeLocation',
        title: 'Change Location',
        type: ComponentType.ChangeLocation
      },
      {
        text: 'Inquiry',
        path: '/inquiry',
        title: 'Inquiry',
        type: ComponentType.Inquiry
      },
      {
        text: 'Serial Documents',
        path: '/serialDocuments',
        title: 'Serial Documents',
        type: ComponentType.SerialDocuments
      },
      {
        text: 'Change Serial',
        path: '/changeSerial',
        title: 'Change Serial',
        type: ComponentType.ChangeSerial
      },
      {
        text: 'Reports',
        path: '/reports',
        title: 'Reports',
        type: ComponentType.Reports
      },
      {
        text: 'Bulk Transfer',
        path: '/bulkTransfer',
        title: 'Bulk Transfer',
        type: ComponentType.BulkTransfer
      },
    ]
  },
  {
    text: 'Sales Order',
    icon: 'folder',
    items: [
      {
        text: 'mdat',
        path: '/mdat',
        title: 'mdat',
        type: ComponentType.Mdat
      },
      {
        text: 'equipment',
        path: '/equipment',
        title: 'eqipment',
        type: ComponentType.ER
      },
      {
        text: 'Shipment',
        path: '/shipment',
        title: 'Shipment',
        type: ComponentType.Shipment
      },
      {
        text: 'Reports',
        path: '/report2',
        title: 'Reports',
        type: ComponentType.Report2
      },
    ]
  }

];
