
export interface LatLng {
    latitude: number
    longitude: number
}

export interface Polyline {
    coordinates: LatLng[]
    strokeWidth: number;
    strokeColor: string;
}

export interface Marker {
    title: string;
    image: string;
    coordinates: LatLng;
    intersectingRouteIndices: number[]
}

export interface MapData {
    lines: Polyline[]
    markers: Marker[];
}

export interface Journey {
    id: number
    name: string
    icon: string
    start: LatLng
    startName: string
    end: LatLng
    endName: string
    startTime: string
}

export interface RouteInfo {
    colorInHex: string,
    routeLabel: string,
    pollutionPoint: number,
    pollutionZone: number,
    duration: string,
    travelCost: number,
    schoolCount: number,
    distance: number,
    modeOfTransport: string
}

export interface JourneySettings {
    journey: Journey | null
    showPollution: boolean,
    showSchools: boolean,
    startTime: string,
    setJourney: (journey: Journey) => void,
    togglePollution: (showPollution: boolean) => void,
    toggleSchools: (showSchools: boolean) => void,
    setStartTime: (startTime: string) => void
}

export const theOneGoodJourney = {
    id: 1,
    name: "Home to Work",
    icon: "business",
    start: {
        latitude: 51.4511732, longitude: -0.2138706
    },
    startName: "Westminster",
    end: {
        latitude: 51.5250836, longitude: -0.0769465
    },
    endName: "North Greenwich",
    startTime: "08:30"
};

export const myJourneys: Journey[] = [
    theOneGoodJourney,
    {
        id: 2,
        name: "Work to Heathrow",
        icon: "business",
        start: {
            latitude: 51.4511731, longitude: -0.2138706
        },
        startName: "North Greenwich",
        end: {
            latitude: 51.5250836, longitude: -0.0769465
        },
        endName: "Heathrow",
        startTime: "08:30"
    }
];
