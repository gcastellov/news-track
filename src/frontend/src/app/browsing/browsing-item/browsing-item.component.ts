import { Component, Input } from '@angular/core';
import { BrowsingElement } from '../browsing-draft/browsing-element';
import { DraftRequestDto } from '../../services/Dtos/DraftRequestDto';

@Component({
    template: ``,
    styleUrls: ['./browsing-item.component.less']
})
export class BrowsingItemComponent {

    @Input()
    element: BrowsingElement | undefined;

    @Input()
    index: number = 0;

    @Input()
    model: DraftRequestDto | undefined;
}
