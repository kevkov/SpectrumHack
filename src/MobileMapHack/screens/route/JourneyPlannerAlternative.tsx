import React, {useEffect, useState, useContext} from 'react';
import {Card, CardItem, Content, Text, Body, View, Toast, Button, Icon, Spinner, CheckBox, ListItem} from "native-base";
import {FlatList, StyleSheet} from 'react-native';
import {
    JourneyParams,
    BusLeg,
    CycleLeg,
    JourneyAlternative,
    TubeLeg,
    WalkingLeg,
    NationalRailLeg, DlrLeg, TramLeg, OvergroundLeg
} from '../../domain/types';
import {api} from '../../api';
import JourneyContext from '../../context/JourneyContext';
import {fromNullable} from "fp-ts/lib/Option";

export const JourneyPlannerAlternative = () => {

    const {journeyPlannerParams, setJourneyPlannerParams} = useContext(JourneyContext);
    const [showAlternativeJourney, setShowAlternativeJourney] = useState(false);
    const [useBus, setUseBus] = useState(true);
    const [useTube, setUseTube] = useState(true);
    const [useOverground, setUseOverground] = useState(true);
    const [useDlr, setUseDlr] = useState(true);
    const [useNationalRail, setUseNationalRail] = useState(true);
    const [useTram, setUseTram] = useState(true);
    const [useRiverBus, setUseRiverBus] = useState(true);
    const [alternativeJourney, setAlternativeJourney] = useState<JourneyAlternative>(null);
    const [loading, setLoading] = useState(false);

    const styles = StyleSheet.create({
        headerText: {
            fontWeight: '600'
        },
        content: {
            padding: 10,
        },
        detailItem: {
            padding: 5
        }
    });

    const defaultModes = ['walking'];

    function calculateModes(): string {
        let modes = new Array(...defaultModes);
        if (useBus) modes.push("bus");
        if (useTube) modes.push("tube");
        if (useOverground) modes.push("overground");
        if (useDlr) modes.push("dlr");
        if (useNationalRail) modes.push("national-rail");
        if (useTram) modes.push("tram");
        if (useRiverBus) modes.push("river-bus");
        return modes.join(",");
    }

    useEffect(() => {
        if (journeyPlannerParams != null) {
            // https://gladysint-insights-func.azurewebsites.net/api/JourneyOptions?code=Qa7a602gQhSdO8i4oCgAf5gv9flmxNUKCqyfa3rAakhwUOPiAuIkHw==&startDateTime=2019-10-04T10:00:00&startLongitude=${journeyPlannerParams.startLongitude}&startLatitude=${journeyPlannerParams.startLatitude}&endLongitude=${journeyPlannerParams.endLongitude}&endLatitude=${journeyPlannerParams.endLatitude}&mode=bus`;
            const url = `http://10.0.2.2:7071/api/JourneyOptions?code=JQRdUTTSpg10FHk0hHiBWloMsRcGZa1ErdcCtFd96uZ2vqzcI9jvug==&startDateTime=${new Date().toISOString()}&startLongitude=${journeyPlannerParams.startLongitude}&startLatitude=${journeyPlannerParams.startLatitude}&endLongitude=${journeyPlannerParams.endLongitude}&endLatitude=${journeyPlannerParams.endLatitude}&mode=${calculateModes()}`;
            console.log('Calling api at: ' + url);

            setLoading(true);
            api<JourneyAlternative>(url)
                .then(data => {
                    console.log("*********** called api, setting journey planner data.");
                    console.log(data.legs);
                    setAlternativeJourney(data);
                    setLoading(false);
                })
                .catch(reason => {
                    console.log(`***********  error calling map api: ${reason}`);
                    // todo: toast does not show immediately
                    Toast.show({
                        text: "There was a problem getting the route details",
                        position: "bottom",
                        type: "warning"
                    });
                    setLoading(false);
                });
        }
    }, [journeyPlannerParams, useBus, useTube, useOverground, useNationalRail, useDlr, useTram]);

    function modeChecks() {
        return (
            <View style={{flexDirection: "row", justifyContent: "flex-start", flexWrap: "wrap"}}>
                <ListItem style={{margin: 0}}><CheckBox checked={useBus} onPress={() => setUseBus(!useBus)} /><Text>Bus</Text></ListItem>
                <ListItem><CheckBox checked={useTube} onPress={() => setUseTube(!useTube)} /><Text>Tube</Text></ListItem>
                <ListItem><CheckBox checked={useOverground} onPress={() => setUseOverground(!useOverground)} /><Text>Overground</Text></ListItem>
                <ListItem><CheckBox checked={useNationalRail} onPress={() => setUseNationalRail(!useNationalRail)} /><Text>Rail</Text></ListItem>
                <ListItem><CheckBox checked={useDlr} onPress={() => setUseDlr(!useDlr)} /><Text>DLR</Text></ListItem>
                <ListItem><CheckBox checked={useTram} onPress={() => setUseTram(!useTram)} /><Text>Tram</Text></ListItem>
            </View>
        );
    }

    const renderBusLeg = (leg: BusLeg) => {
        return (<Card>
            <CardItem bordered>
                <Icon name="bus" style={{color: 'red'}}/>
                <Text style={styles.headerText}>Bus</Text>
            </CardItem>
            <CardItem style={{backgroundColor: '#eeeeee'}}>
                <Body>
                    <View>
                        <Text style={styles.detailItem}>Route: {leg.routeNumber}</Text>
                        <Text style={styles.detailItem}>Get on at : {leg.startPoint}</Text>
                        <Text style={styles.detailItem}>Get off at : {leg.arrivalPoint}</Text>
                    </View>
                </Body>
            </CardItem>
        </Card>);
    };

    const renderWalkingLeg = (leg: WalkingLeg) => {
        return (<Card>
            <CardItem bordered>
                <Icon name="walk" />
                <Text style={styles.headerText}>Walk</Text>
            </CardItem>
            <CardItem style={{backgroundColor: '#eeeeee'}}>
                <Body>
                    <View>
                        <Text style={styles.detailItem}>Instruction: {leg.details}</Text>
                        <Text style={styles.detailItem}>Distance: {leg.distance} m</Text>
                    </View>
                </Body>
            </CardItem>
        </Card>);
    };

    const renderCycleLeg = (leg: CycleLeg) => {
        return (<Card>
            <CardItem bordered>
                <Icon name="bicycle"  style={{color: 'green'}}/>
                <Text style={styles.headerText}>Cycle</Text>
            </CardItem>
            <CardItem style={{backgroundColor: '#eeeeee'}}>
                <Body>
                    <View>
                        <Text style={styles.detailItem}>From : {leg.startPoint}</Text>
                        <Text style={styles.detailItem}>To : {leg.arrivalPoint}</Text>
                        <Text style={styles.detailItem}>Distance: {leg.distance} m</Text>
                        <Text style={styles.detailItem}>Duration: {leg.duration} min</Text>
                    </View>
                </Body>
            </CardItem>
        </Card>);
    };

    const renderTubeLeg = (leg: TubeLeg) => {
        return (<Card>
            <CardItem bordered>
                <Icon name="train" style={{color: 'blue'}}/>
                <Text style={styles.headerText}>Tube</Text>
            </CardItem>
            <CardItem style={{backgroundColor: '#eeeeee'}}>
                <Body>
                    <View>
                        <Text style={styles.detailItem}>Route: {leg.routeName}</Text>
                        <Text style={styles.detailItem}>Get on at : {leg.startPoint}</Text>
                        <Text style={styles.detailItem}>Get off at : {leg.arrivalPoint}</Text>
                    </View>
                </Body>
            </CardItem>
        </Card>);
    };

    const renderRailLeg = (leg: NationalRailLeg) => {
        return (<Card>
            <CardItem bordered>
                <Icon name="train" style={{color: 'black'}}/>
                <Text style={styles.headerText}>National Rail</Text>
            </CardItem>
            <CardItem style={{backgroundColor: '#eeeeee'}}>
                <Body>
                    <View>
                        <Text style={styles.detailItem}>Route: {leg.summary}</Text>
                        <Text style={styles.detailItem}>Get on at : {leg.startPoint}</Text>
                        <Text style={styles.detailItem}>Get off at : {leg.arrivalPoint}</Text>
                    </View>
                </Body>
            </CardItem>
        </Card>);
    };

    const renderDlrLeg = (leg: DlrLeg) => {
        return (<Card>
            <CardItem bordered>
                <Icon name="train" style={{color: 'black'}}/>
                <Text style={styles.headerText}>DLR</Text>
            </CardItem>
            <CardItem style={{backgroundColor: '#eeeeee'}}>
                <Body>
                    <View>
                        <Text style={styles.detailItem}>Route: {leg.summary}</Text>
                        <Text style={styles.detailItem}>Get on at : {leg.startPoint}</Text>
                        <Text style={styles.detailItem}>Get off at : {leg.arrivalPoint}</Text>
                    </View>
                </Body>
            </CardItem>
        </Card>);
    };

    const renderTramLeg = (leg: DlrLeg) => {
        return (<Card>
            <CardItem bordered>
                <Icon name="train" style={{color: 'black'}}/>
                <Text style={styles.headerText}>Overground</Text>
            </CardItem>
            <CardItem style={{backgroundColor: '#eeeeee'}}>
                <Body>
                    <View>
                        <Text style={styles.detailItem}>Route: {leg.summary}</Text>
                        <Text style={styles.detailItem}>Get on at : {leg.startPoint}</Text>
                        <Text style={styles.detailItem}>Get off at : {leg.arrivalPoint}</Text>
                    </View>
                </Body>
            </CardItem>
        </Card>);
    };

    const renderOvergroundLeg = (leg: OvergroundLeg) => {
        return (<Card>
            <CardItem bordered>
                <Icon name="train" style={{color: 'black'}}/>
                <Text style={styles.headerText}>Tram</Text>
            </CardItem>
            <CardItem style={{backgroundColor: '#eeeeee'}}>
                <Body>
                    <View>
                        <Text style={styles.detailItem}>Route: {leg.summary}</Text>
                        <Text style={styles.detailItem}>Get on at : {leg.startPoint}</Text>
                        <Text style={styles.detailItem}>Get off at : {leg.arrivalPoint}</Text>
                    </View>
                </Body>
            </CardItem>
        </Card>);
    };

    const renderUnknownLeg = (mode: string) => {
        return (<Card>
            <CardItem bordered>
                <Text style={styles.headerText}>{mode}</Text>
            </CardItem>
        </Card>);
    };

    const toggleAlternativeJourney = () => {

        if (journeyPlannerParams === null) {
            setJourneyPlannerParams(JourneyParams);
            setAlternativeJourney(null);
            setLoading(true);
        }
        setShowAlternativeJourney(!showAlternativeJourney);
    };

    console.log("rendering journey card");

    return (
        <Content style={styles.content}>
            <Card>
                <CardItem bordered style={{backgroundColor: 'red'}}>
                    <Text style={styles.headerText}>Journey Summary</Text>
                </CardItem>
                <CardItem style={{backgroundColor: '#eeeeee'}}>
                    <Body>
                        <View>
                            <Text style={styles.detailItem}>From: 24 Sutherland Square</Text>
                            <Text style={styles.detailItem}>To: 144 Queen Victoria St</Text>
                            <Text style={styles.detailItem}>Distance: 2.5 miles</Text>
                            <Text style={styles.detailItem}>Cost: £5.40</Text>
                            <Text style={styles.detailItem}>Duration: 18 min</Text>
                        </View>
                    </Body>
                </CardItem>
                <CardItem button style={{justifyContent: 'center'}}>
                    <Button iconLeft small onPress={() => toggleAlternativeJourney()}>
                        <Text>Alternative Journey</Text>
                    </Button>
                </CardItem>
                {showAlternativeJourney ? modeChecks() : null}
            </Card>
            {loading ? (<View style={{justifyContent: 'center', backgroundColor: "transparent"}}><Spinner /></View>) :
                showAlternativeJourney && alternativeJourney &&
                <View>
                    <FlatList
                        data={alternativeJourney.legs}
                        keyExtractor={(item, index) => index.toString()}
                        renderItem={datum =>
                            datum.item.mode == 'bus' ? renderBusLeg(datum.item as BusLeg)
                                : (datum.item.mode == 'walking' ? renderWalkingLeg(datum.item as WalkingLeg)
                                : (datum.item.mode == 'cycle' ? renderCycleLeg(datum.item as CycleLeg)
                                : (datum.item.mode == 'tube' ? renderTubeLeg(datum.item as TubeLeg)
                                : (datum.item.mode == 'national-rail' ? renderRailLeg(datum.item as NationalRailLeg)
                                : (datum.item.mode == 'dlr' ? renderDlrLeg(datum.item as DlrLeg)
                                : (datum.item.mode == 'tram' ? renderTramLeg(datum.item as TramLeg)
                                : (datum.item.mode == 'overground' ? renderOvergroundLeg(datum.item as OvergroundLeg)
                                : renderUnknownLeg(datum.item.mode))))))))}
                    />
                </View>}
        </Content>)

};
