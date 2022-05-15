
export interface DeleteFile {
  id: number;
  name: string;
}

export class DeleteFileFromOrderVM {
  private fileName: string;
  private location: string; 
  private orderId: number;
  private productId: number;

  constructor(
    $fileName: string,
    $location: string, 
    $orderId: number,
    $productId: number
  ) {
    this.fileName = $fileName;
    this.location = $location; 
    this.orderId = $orderId;
    this.productId = $productId;
  }
}
