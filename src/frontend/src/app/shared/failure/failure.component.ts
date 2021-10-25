import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-failure',
  templateUrl: './failure.component.html',
  styleUrls: ['./failure.component.less']
})
export class FailureComponent {

  @Input()
  errorMessage: string = '';
}
